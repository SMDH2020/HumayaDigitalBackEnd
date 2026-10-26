-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Revision y aprobacion POR FUNCIONALIDAD
-- (reemplaza la revision/aprobacion a nivel de solicitud general que se
-- habia implementado antes). Script preparado para REVISION Y EJECUCION
-- MANUAL. No se ha ejecutado contra ningun servidor.
--
-- Motivo del cambio: cada funcionalidad del alcance debe poder enviarse a
-- revision y ser aprobada/rechazada por el solicitante de forma
-- INDEPENDIENTE de las demas, en vez de una sola decision que afectaba a
-- toda la solicitud (EN_REVISION_PRIORIDAD / TERMINADA en TI_Solicitudes).
-- Esa transicion de estado general de la solicitud YA NO SE USA para esto
-- -- su SP (sp_TI_Solicitudes_ResolverRevisionPrioridad) y los estados del
-- catalogo (EN_REVISION_PRIORIDAD, TERMINADA) NO se eliminan ni se tocan,
-- simplemente la pantalla ya no los dispara para este flujo.
--
-- Persistencia: NO se crea ninguna tabla ni columna nueva. Se reutiliza
-- dbo.TI_CambiosProgramacion (ya instalada en 08_Migracion_ProgramacionMensual.sql)
-- porque su esquema ya cubre exactamente lo necesario para una revision por
-- funcionalidad:
--   FuncionalidadId, SolicitudId          -> a que funcionalidad/solicitud aplica
--   SolicitadoPorUsuarioId, FechaSolicitud -> quien la envio a revision y cuando
--   Motivo                                 -> por que se envio (texto fijo aqui,
--                                              el motivo "de negocio" relevante
--                                              es el de rechazo, no el de envio)
--   EstadoCambio ('PENDIENTE'/'APROBADO'/'RECHAZADO') -> estado de la revision
--   DecididoPorUsuarioId, FechaDecision, ComentarioResolucion -> quien y cuando
--                                              aprobo/rechazo, y el motivo del
--                                              rechazo
--
-- Cuando Jefatura de TI envia una funcionalidad a revision, se inserta una
-- fila PENDIENTE con MesPropuesto/AnioPropuesto = el MesProgramado/
-- AnioProgramado YA vigente (no es una propuesta de cambio real, es una
-- revision sobre lo ya programado). Esto permite derivar el estado de
-- revision vigente SIN ninguna columna adicional: se toma la fila mas
-- reciente de TI_CambiosProgramacion para esa funcionalidad y se compara
-- su Mes/AnioPropuesto (o MesAnterior/AnioAnteriorProgramado si fue
-- rechazada) contra el MesProgramado/AnioProgramado ACTUAL de la
-- funcionalidad:
--   - Si coincide y EstadoCambio = 'APROBADO'  -> "Aprobada por el usuario"
--   - Si coincide y EstadoCambio = 'RECHAZADO' -> "Revision rechazada"
--   - Si EstadoCambio = 'PENDIENTE'            -> "En revision" (bloqueada)
--   - Si NO coincide (Jefatura de TI reprogramo la funcionalidad despues
--     de esa resolucion, cambiando mes/anio) -> esa resolucion anterior ya
--     no aplica al mes/anio actual, se ignora automaticamente y la fila se
--     muestra en su estado normal de backlog/programada, lista para
--     enviarse de nuevo a revision.
--
-- La regla "una funcionalidad con revision PENDIENTE no se puede
-- reprogramar/regresar a backlog" se refuerza tambien dentro de
-- sp_TI_Funcionalidades_ProgramarInicial y sp_TI_Funcionalidades_RegresarABacklog
-- (ver mas abajo) -- no basta con ocultar los botones en el frontend.
--
-- Seguridad:
--   - sp_TI_Funcionalidades_EnviarARevision la ejecuta Jefatura de TI (el
--     controller la gatea por rol, igual que ProgramarInicial/
--     RegresarABacklog).
--   - sp_TI_Funcionalidades_ResolverRevision valida la identidad REAL del
--     usuario (@usuarioId = TI_Solicitudes.SolicitanteUsuarioId de la
--     solicitud duena de la funcionalidad), nunca un rol -- mismo patron
--     que sp_TI_Solicitudes_ResolverRevisionPrioridad. Gerencia de
--     Planeacion Estrategica no aparece en ninguna validacion de este
--     archivo: sigue sin poder aprobar ni modificar nada de este flujo.
--   - Update atomico (UPDATE...OUTPUT WHERE EstadoCambio = 'PENDIENTE')
--     para evitar la carrera de doble resolucion, mismo patron que
--     sp_TI_CambiosProgramacion_Resolver.
--
-- Contenido (todo CREATE OR ALTER PROCEDURE, idempotente; nada de esto crea
-- tablas ni columnas):
--   1. sp_TI_Funcionalidades_ProgramarInicial    -- se agrega el bloqueo si
--      hay una revision PENDIENTE.
--   2. sp_TI_Funcionalidades_RegresarABacklog    -- idem.
--   3. sp_TI_Funcionalidades_ProgramacionVigente_ListadoPorSolicitud --
--      ahora devuelve tambien el estado de revision vigente por fila.
--   4. sp_TI_Funcionalidades_EnviarARevision     -- NUEVO.
--   5. sp_TI_Funcionalidades_ResolverRevision    -- NUEVO.
--
-- Estos 5 procedimientos quedan definidos de forma identica (cuerpo
-- completo, ya reinstalado) en 09_ProgramacionMensual.sql, que es el
-- archivo fuente canonico de este bloque. Este script aislado existe solo
-- para poder instalar unicamente esta actualizacion sin volver a ejecutar
-- el archivo completo. Es seguro ejecutarlo sin importar si
-- 11_Instalacion_PrioridadPorFuncionalidad.sql ya se corrio antes.
--
-- ORDEN DE INSTALACION: ejecutar los 5 bloques de este archivo en el orden
-- en que aparecen.
-- ============================================================================

-- ----------------------------------------------------------------------------
-- 1) sp_TI_Funcionalidades_ProgramarInicial (+ bloqueo por revision pendiente)
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_ProgramarInicial
    @FuncionalidadId BIGINT,
    @MesProgramado TINYINT,
    @AnioProgramado SMALLINT,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@MesProgramado IS NULL OR @MesProgramado NOT BETWEEN 1 AND 12)
        BEGIN
            RAISERROR('El mes programado debe estar entre 1 y 12', 16, 1);
            RETURN;
        END
        IF (@AnioProgramado IS NULL)
        BEGIN
            RAISERROR('El anio programado es obligatorio', 16, 1);
            RETURN;
        END

        DECLARE @SolicitudId BIGINT, @MesActual TINYINT, @AnioActual SMALLINT, @VersionAlcanceId BIGINT;
        SELECT @SolicitudId = v.SolicitudId, @MesActual = f.MesProgramado, @AnioActual = f.AnioProgramado, @VersionAlcanceId = f.VersionAlcanceId
        FROM dbo.TI_Funcionalidades f
        INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId
        WHERE f.FuncionalidadId = @FuncionalidadId AND f.Activa = 1;

        IF (@SolicitudId IS NULL)
        BEGIN
            RAISERROR('La funcionalidad indicada no existe o no esta activa', 16, 1);
            RETURN;
        END

        DECLARE @VersionVigenteId BIGINT = (
            SELECT TOP 1 VersionAlcanceId FROM dbo.TI_VersionesAlcance
            WHERE SolicitudId = @SolicitudId AND EstadoVersion = 'APROBADA'
            ORDER BY NumeroVersion DESC
        );
        IF (@VersionVigenteId IS NULL OR @VersionAlcanceId <> @VersionVigenteId)
        BEGIN
            RAISERROR('La funcionalidad no pertenece a la version de alcance vigente de la solicitud', 16, 1);
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM dbo.TI_CambiosProgramacion WHERE FuncionalidadId = @FuncionalidadId AND EstadoCambio = 'PENDIENTE')
        BEGIN
            RAISERROR('La funcionalidad tiene una revision pendiente; no se puede modificar su programacion', 16, 1);
            RETURN;
        END

        UPDATE dbo.TI_Funcionalidades
           SET MesProgramado = @MesProgramado, AnioProgramado = @AnioProgramado
         WHERE FuncionalidadId = @FuncionalidadId;

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudId, @usuarioId, 'TI_Funcionalidades', CAST(@FuncionalidadId AS NVARCHAR(200)),
                CASE WHEN @MesActual IS NULL AND @AnioActual IS NULL THEN 'PROGRAMACION_INICIAL' ELSE 'REPROGRAMACION' END,
                SYSUTCDATETIME(),
                CASE WHEN @MesActual IS NULL AND @AnioActual IS NULL THEN NULL ELSE CONCAT('Mes=', @MesActual, ' Anio=', @AnioActual) END,
                CONCAT('Mes=', @MesProgramado, ' Anio=', @AnioProgramado), 'API');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- 2) sp_TI_Funcionalidades_RegresarABacklog (+ bloqueo por revision pendiente)
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_RegresarABacklog
    @FuncionalidadId BIGINT,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @SolicitudId BIGINT, @MesActual TINYINT, @AnioActual SMALLINT;
        SELECT @SolicitudId = v.SolicitudId, @MesActual = f.MesProgramado, @AnioActual = f.AnioProgramado
        FROM dbo.TI_Funcionalidades f
        INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId
        WHERE f.FuncionalidadId = @FuncionalidadId AND f.Activa = 1;

        IF (@SolicitudId IS NULL)
        BEGIN
            RAISERROR('La funcionalidad indicada no existe o no esta activa', 16, 1);
            RETURN;
        END
        IF (@MesActual IS NULL AND @AnioActual IS NULL)
        BEGIN
            RAISERROR('La funcionalidad ya esta en backlog', 16, 1);
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM dbo.TI_CambiosProgramacion WHERE FuncionalidadId = @FuncionalidadId AND EstadoCambio = 'PENDIENTE')
        BEGIN
            RAISERROR('La funcionalidad tiene una revision pendiente; no se puede modificar su programacion', 16, 1);
            RETURN;
        END

        UPDATE dbo.TI_Funcionalidades SET MesProgramado = NULL, AnioProgramado = NULL WHERE FuncionalidadId = @FuncionalidadId;

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudId, @usuarioId, 'TI_Funcionalidades', CAST(@FuncionalidadId AS NVARCHAR(200)), 'REGRESO_A_BACKLOG', SYSUTCDATETIME(),
                CONCAT('Mes=', @MesActual, ' Anio=', @AnioActual), NULL, 'API');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- 3) sp_TI_Funcionalidades_ProgramacionVigente_ListadoPorSolicitud
--    (+ estado de revision vigente por funcionalidad)
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_ProgramacionVigente_ListadoPorSolicitud
    @SolicitudId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @VersionAlcanceId BIGINT = (
        SELECT TOP 1 VersionAlcanceId FROM dbo.TI_VersionesAlcance
        WHERE SolicitudId = @SolicitudId AND EstadoVersion = 'APROBADA'
        ORDER BY NumeroVersion DESC
    );

    ;WITH UltimoCambio AS (
        SELECT c.*, ROW_NUMBER() OVER (PARTITION BY c.FuncionalidadId ORDER BY c.FechaSolicitud DESC) AS Orden_
        FROM dbo.TI_CambiosProgramacion c
        WHERE c.SolicitudId = @SolicitudId
    )
    SELECT f.FuncionalidadId, f.VersionAlcanceId, f.Codigo, f.Nombre, f.DiasEstimados, f.MesProgramado, f.AnioProgramado,
           uc.CambioProgramacionId AS RevisionCambioProgramacionId,
           CASE
               WHEN uc.EstadoCambio = 'PENDIENTE' THEN 'EN_REVISION'
               WHEN uc.EstadoCambio = 'APROBADO' AND uc.MesPropuesto = f.MesProgramado AND uc.AnioPropuesto = f.AnioProgramado THEN 'APROBADA'
               WHEN uc.EstadoCambio = 'RECHAZADO' AND uc.MesAnteriorProgramado = f.MesProgramado AND uc.AnioAnteriorProgramado = f.AnioProgramado THEN 'RECHAZADA'
               ELSE NULL
           END AS RevisionEstado,
           uc.SolicitadoPorUsuarioId AS RevisionSolicitadoPorUsuarioId,
           uc.FechaSolicitud AS RevisionFechaSolicitud,
           uc.DecididoPorUsuarioId AS RevisionDecididoPorUsuarioId,
           uc.FechaDecision AS RevisionFechaDecision,
           uc.ComentarioResolucion AS RevisionComentarioResolucion
    FROM dbo.TI_Funcionalidades f
    LEFT JOIN UltimoCambio uc ON uc.FuncionalidadId = f.FuncionalidadId AND uc.Orden_ = 1
    WHERE f.VersionAlcanceId = @VersionAlcanceId AND f.Activa = 1
    ORDER BY f.Orden;
END
GO

-- ----------------------------------------------------------------------------
-- 4) sp_TI_Funcionalidades_EnviarARevision (nuevo)
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_EnviarARevision
    @FuncionalidadId BIGINT,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @SolicitudId BIGINT, @MesProgramado TINYINT, @AnioProgramado SMALLINT, @VersionAlcanceId BIGINT;
        SELECT @SolicitudId = v.SolicitudId, @MesProgramado = f.MesProgramado, @AnioProgramado = f.AnioProgramado, @VersionAlcanceId = f.VersionAlcanceId
        FROM dbo.TI_Funcionalidades f
        INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId
        WHERE f.FuncionalidadId = @FuncionalidadId AND f.Activa = 1;

        IF (@SolicitudId IS NULL)
        BEGIN
            RAISERROR('La funcionalidad indicada no existe o no esta activa', 16, 1);
            RETURN;
        END

        DECLARE @VersionVigenteId BIGINT = (
            SELECT TOP 1 VersionAlcanceId FROM dbo.TI_VersionesAlcance
            WHERE SolicitudId = @SolicitudId AND EstadoVersion = 'APROBADA'
            ORDER BY NumeroVersion DESC
        );
        IF (@VersionVigenteId IS NULL OR @VersionAlcanceId <> @VersionVigenteId)
        BEGIN
            RAISERROR('La funcionalidad no pertenece a la version de alcance vigente de la solicitud', 16, 1);
            RETURN;
        END

        IF (@MesProgramado IS NULL OR @AnioProgramado IS NULL)
        BEGIN
            RAISERROR('La funcionalidad debe tener mes y anio programados antes de enviarla a revision', 16, 1);
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM dbo.TI_CambiosProgramacion WHERE FuncionalidadId = @FuncionalidadId AND EstadoCambio = 'PENDIENTE')
        BEGIN
            RAISERROR('Esta funcionalidad ya tiene una revision pendiente', 16, 1);
            RETURN;
        END

        INSERT INTO dbo.TI_CambiosProgramacion (FuncionalidadId, SolicitudId, MesAnteriorProgramado, AnioAnteriorProgramado, MesPropuesto, AnioPropuesto, SolicitadoPorUsuarioId, FechaSolicitud, Motivo, EstadoCambio)
        VALUES (@FuncionalidadId, @SolicitudId, @MesProgramado, @AnioProgramado, @MesProgramado, @AnioProgramado, @usuarioId, SYSUTCDATETIME(), 'Enviado a revision de prioridad por Jefatura de TI', 'PENDIENTE');

        DECLARE @CambioProgramacionId BIGINT = SCOPE_IDENTITY();

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudId, @usuarioId, 'TI_Funcionalidades', CAST(@FuncionalidadId AS NVARCHAR(200)), 'FUNCIONALIDAD_ENVIADA_REVISION', SYSUTCDATETIME(),
                NULL, CONCAT('Mes=', @MesProgramado, ' Anio=', @AnioProgramado), 'API');

        COMMIT TRANSACTION;
        SELECT @CambioProgramacionId AS CambioProgramacionId;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- 5) sp_TI_Funcionalidades_ResolverRevision (nuevo)
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_ResolverRevision
    @CambioProgramacionId BIGINT,
    @EstadoResolucion NVARCHAR(20),   -- 'APROBADO' | 'RECHAZADO'
    @ComentarioResolucion NVARCHAR(2000) = NULL,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@EstadoResolucion IS NULL OR @EstadoResolucion NOT IN ('APROBADO', 'RECHAZADO'))
        BEGIN
            RAISERROR('El estado de resolucion debe ser APROBADO o RECHAZADO', 16, 1);
            RETURN;
        END
        IF (@EstadoResolucion = 'RECHAZADO' AND (@ComentarioResolucion IS NULL OR LTRIM(RTRIM(@ComentarioResolucion)) = ''))
        BEGIN
            RAISERROR('El motivo es obligatorio para rechazar la revision', 16, 1);
            RETURN;
        END

        DECLARE @SolicitudId BIGINT, @FuncionalidadId BIGINT, @SolicitanteUsuarioId INT;
        SELECT @SolicitudId = c.SolicitudId, @FuncionalidadId = c.FuncionalidadId
        FROM dbo.TI_CambiosProgramacion c
        WHERE c.CambioProgramacionId = @CambioProgramacionId;

        IF (@SolicitudId IS NULL)
        BEGIN
            RAISERROR('La revision indicada no existe', 16, 1);
            RETURN;
        END

        SELECT @SolicitanteUsuarioId = s.SolicitanteUsuarioId FROM dbo.TI_Solicitudes s WHERE s.SolicitudId = @SolicitudId;

        IF (@usuarioId <> @SolicitanteUsuarioId)
        BEGIN
            RAISERROR('Solo el usuario solicitante de esta solicitud puede resolver la revision de la funcionalidad', 16, 1);
            RETURN;
        END

        DECLARE @Resuelto TABLE (FuncionalidadId BIGINT);

        UPDATE dbo.TI_CambiosProgramacion
           SET EstadoCambio = @EstadoResolucion, DecididoPorUsuarioId = @usuarioId, FechaDecision = SYSUTCDATETIME(), ComentarioResolucion = @ComentarioResolucion
        OUTPUT inserted.FuncionalidadId
        INTO @Resuelto
         WHERE CambioProgramacionId = @CambioProgramacionId AND EstadoCambio = 'PENDIENTE';

        IF (SELECT COUNT(*) FROM @Resuelto) = 0
        BEGIN
            RAISERROR('La revision no existe o ya fue resuelta anteriormente', 16, 1);
            RETURN;
        END

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudId, @usuarioId, 'TI_Funcionalidades', CAST(@FuncionalidadId AS NVARCHAR(200)),
                CASE WHEN @EstadoResolucion = 'APROBADO' THEN 'FUNCIONALIDAD_REVISION_APROBADA' ELSE 'FUNCIONALIDAD_REVISION_RECHAZADA' END,
                SYSUTCDATETIME(), NULL, @ComentarioResolucion, 'API');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
