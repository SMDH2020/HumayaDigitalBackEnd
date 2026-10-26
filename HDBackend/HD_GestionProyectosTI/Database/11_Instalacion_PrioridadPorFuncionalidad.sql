-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Instalacion del flujo de prioridad por
-- funcionalidad y su revision/aprobacion (detalle de solicitud).
-- Script preparado para REVISION Y EJECUCION MANUAL. No se ha ejecutado
-- contra ningun servidor.
--
-- REQUIERE (ya aplicado segun confirmacion del responsable):
--   - 07_Migracion_DiasEstimados.sql / 08_Migracion_ProgramacionMensual.sql
--     (columnas TI_Funcionalidades.MesProgramado/AnioProgramado).
--   - Catalogo dbo.TI_EstadosSolicitud con las filas:
--       EN_REVISION_PRIORIDAD (EstadoSolicitudId=21, Etapa=PLANEACION, Orden=115)
--       TERMINADA              (EstadoSolicitudId=22, Etapa=PLANEACION, Orden=118)
--     Este script NO vuelve a insertar esas filas -- ya estan confirmadas.
--
-- Contenido (solo lo necesario para activar el flujo acordado; nada de esto
-- crea tablas ni columnas, todo es CREATE OR ALTER PROCEDURE, idempotente):
--   1. sp_TI_Solicitudes_CambiarEstado -- cuerpo completo con la whitelist
--      ampliada (unica rama nueva usada aqui: PENDIENTE_PRIORIZACION ->
--      EN_REVISION_PRIORIDAD). Es el mismo cuerpo ya instalado en turnos
--      anteriores de Fase 2A/2B; se reinstala completo porque CREATE OR
--      ALTER PROCEDURE no admite "agregar solo una rama".
--   2. sp_TI_Funcionalidades_ProgramarInicial -- ahora es upsert directo
--      (programa Y reprograma) por Jefatura de TI, sin decision de
--      solicitud ni aprobacion de Planeacion.
--   3. sp_TI_Funcionalidades_RegresarABacklog -- limpia MesProgramado/
--      AnioProgramado, accion directa de Jefatura de TI.
--   4. sp_TI_Funcionalidades_ProgramacionVigente_ListadoPorSolicitud --
--      sin cambios de logica; se reinstala para que este script quede
--      autocontenido (ya es el que usa el endpoint de listado).
--   5. sp_TI_Solicitudes_ResolverRevisionPrioridad -- NUEVO. El unico
--      camino para resolver EN_REVISION_PRIORIDAD: valida identidad real
--      del solicitante (@usuarioId = TI_Solicitudes.SolicitanteUsuarioId,
--      nunca un rol) y el estado actual; aprobar -> TERMINADA, rechazar
--      (motivo obligatorio) -> PENDIENTE_PRIORIZACION. Transaccional
--      (SET XACT_ABORT ON + TRY/CATCH + ROLLBACK), registra
--      TI_SolicitudEstadosHistorial y TI_AuditoriaEventos.
--
-- Seguridad verificada antes de entregar este script:
--   - El endpoint generico SolicitudesTI/CambiarEstado NO puede alcanzar
--     TERMINADA ni devolver a PENDIENTE_PRIORIZACION desde
--     EN_REVISION_PRIORIDAD: esas transiciones NO estan en la whitelist de
--     sp_TI_Solicitudes_CambiarEstado (abajo) a proposito. Solo existen
--     dentro de sp_TI_Solicitudes_ResolverRevisionPrioridad, que ademas
--     revalida la identidad del solicitante independientemente de cualquier
--     rol -- Jefatura de TI, aunque tenga permiso para llamar al endpoint
--     generico, no puede usarlo para llegar a TERMINADA ni para resolver
--     la revision en nombre del solicitante.
--   - Gerencia de Planeacion Estrategica no aparece en ninguna validacion
--     de este script: no puede aprobar ni editar nada de este flujo
--     (su acceso de solo lectura se mantiene porque ninguna de estas
--     acciones la autoriza).
--   - sp_TI_CambiosProgramacion_Crear/_Resolver, sp_TI_DecisionesPrioridad_Crear,
--     sp_TI_AprobacionesPrioridad_Registrar y sus tablas NO se tocan ni se
--     eliminan en este script -- siguen instalados, simplemente el flujo
--     nuevo no los invoca.
--   - Todas las operaciones usan Dapper + stored procedure desde C#
--     (FactoryConection + CommandType.StoredProcedure); no hay DML directo
--     desde el backend.
--
-- ORDEN DE INSTALACION: ejecutar los 5 bloques de este archivo en el orden
-- en que aparecen (de arriba hacia abajo). No depende de ningun orden
-- especial entre si fuera de eso.
-- ============================================================================

-- ----------------------------------------------------------------------------
-- 1) sp_TI_Solicitudes_CambiarEstado (whitelist con EN_REVISION_PRIORIDAD)
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_CambiarEstado
    @SolicitudId      BIGINT,
    @EstadoNuevoCodigo NVARCHAR(100),
    @usuarioId        INT,
    @Motivo           NVARCHAR(4000) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EstadoActualId INT, @EstadoActualCodigo NVARCHAR(100);
    SELECT @EstadoActualId = s.EstadoSolicitudId, @EstadoActualCodigo = es.Codigo
    FROM dbo.TI_Solicitudes s
    INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
    WHERE s.SolicitudId = @SolicitudId;

    IF @EstadoActualId IS NULL
    BEGIN
        RAISERROR('Solicitud no encontrada', 16, 1);
        RETURN;
    END

    IF (@EstadoNuevoCodigo IN ('RECHAZADA', 'CANCELADA') AND (@Motivo IS NULL OR LTRIM(RTRIM(@Motivo)) = ''))
    BEGIN
        RAISERROR('El motivo es obligatorio para este cambio de estado', 16, 1);
        RETURN;
    END

    DECLARE @Permitido BIT = 0;
    -- Catalogo de transiciones habilitadas en Fase 2A. Ampliar aqui cuando
    -- se implemente la fase de definicion de alcance en adelante.
    IF (@EstadoActualCodigo = 'ENVIADA'                AND @EstadoNuevoCodigo IN ('EN_REVISION', 'CANCELADA'))
        SET @Permitido = 1;
    ELSE IF (@EstadoActualCodigo = 'EN_REVISION'        AND @EstadoNuevoCodigo IN ('ESPERANDO_INFORMACION', 'RECHAZADA', 'CANCELADA'))
        SET @Permitido = 1;
    ELSE IF (@EstadoActualCodigo = 'ESPERANDO_INFORMACION' AND @EstadoNuevoCodigo IN ('EN_REVISION', 'CANCELADA'))
        SET @Permitido = 1;
    -- Ampliacion Fase 2B (definicion de alcance, estimacion y priorizacion).
    -- No se modifico ninguna linea de arriba; solo se agregaron estas ramas.
    ELSE IF (@EstadoActualCodigo = 'EN_REVISION' AND @EstadoNuevoCodigo = 'DEFINICION_ALCANCE')
        SET @Permitido = 1;
    ELSE IF (@EstadoActualCodigo = 'DEFINICION_ALCANCE' AND @EstadoNuevoCodigo = 'PENDIENTE_AUTORIZACION_ALCANCE')
        SET @Permitido = 1;
    ELSE IF (@EstadoActualCodigo = 'PENDIENTE_AUTORIZACION_ALCANCE' AND @EstadoNuevoCodigo IN ('EVALUACION_TECNICA', 'PENDIENTE_PRIORIZACION', 'DEFINICION_ALCANCE'))
        SET @Permitido = 1;
    ELSE IF (@EstadoActualCodigo = 'EVALUACION_TECNICA' AND @EstadoNuevoCodigo IN ('PENDIENTE_PRIORIZACION', 'DEFINICION_ALCANCE'))
        SET @Permitido = 1;
    ELSE IF (@EstadoActualCodigo = 'PENDIENTE_PRIORIZACION' AND @EstadoNuevoCodigo IN ('PRIORIZADA', 'EN_BACKLOG', 'RECHAZADA', 'DEFINICION_ALCANCE', 'EN_REVISION_PRIORIDAD'))
        SET @Permitido = 1;
    -- Revision de la priorizacion por funcionalidad (detalle de solicitud):
    -- la entrada a EN_REVISION_PRIORIDAD la dispara Jefatura de TI por este
    -- endpoint generico (arriba). La SALIDA (-> TERMINADA o de vuelta a
    -- PENDIENTE_PRIORIZACION) NO se agrega aqui a proposito: solo la puede
    -- resolver sp_TI_Solicitudes_ResolverRevisionPrioridad, que valida la
    -- identidad real del solicitante -- si se agregara aqui, Jefatura de TI
    -- podria alcanzar TERMINADA directo por este mismo endpoint generico y
    -- saltarse esa aprobacion.
    ELSE IF (@EstadoActualCodigo = 'PRIORIZADA' AND @EstadoNuevoCodigo IN ('EN_BACKLOG', 'RECHAZADA', 'DEFINICION_ALCANCE'))
        SET @Permitido = 1;
    ELSE IF (@EstadoActualCodigo = 'EN_BACKLOG' AND @EstadoNuevoCodigo IN ('PRIORIZADA', 'RECHAZADA', 'DEFINICION_ALCANCE', 'EN_REVISION_PRIORIDAD'))
        SET @Permitido = 1;

    IF (@Permitido = 0)
    BEGIN
        RAISERROR('La transicion de estado solicitada no esta permitida', 16, 1);
        RETURN;
    END

    DECLARE @EstadoNuevoId INT = (SELECT EstadoSolicitudId FROM dbo.TI_EstadosSolicitud WHERE Codigo = @EstadoNuevoCodigo);

    UPDATE dbo.TI_Solicitudes
       SET EstadoSolicitudId = @EstadoNuevoId,
           FechaUltimaActualizacion = SYSUTCDATETIME(),
           MotivoRechazo = CASE WHEN @EstadoNuevoCodigo = 'RECHAZADA' THEN @Motivo ELSE MotivoRechazo END,
           MotivoCancelacion = CASE WHEN @EstadoNuevoCodigo = 'CANCELADA' THEN @Motivo ELSE MotivoCancelacion END,
           FechaCierre = CASE WHEN @EstadoNuevoCodigo IN ('RECHAZADA', 'CANCELADA') THEN SYSUTCDATETIME() ELSE FechaCierre END
     WHERE SolicitudId = @SolicitudId;

    INSERT INTO dbo.TI_SolicitudEstadosHistorial (SolicitudId, EstadoAnteriorId, EstadoNuevoId, UsuarioCambioId, FechaCambio, Motivo, Origen)
    VALUES (@SolicitudId, @EstadoActualId, @EstadoNuevoId, @usuarioId, SYSUTCDATETIME(), @Motivo, 'API');

    INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
    VALUES (@SolicitudId, @usuarioId, 'TI_Solicitudes', CAST(@SolicitudId AS NVARCHAR(200)), 'CAMBIAR_ESTADO', SYSUTCDATETIME(), @EstadoActualCodigo, @EstadoNuevoCodigo, 'API');
END
GO

-- ----------------------------------------------------------------------------
-- 2) sp_TI_Funcionalidades_ProgramarInicial (upsert directo, Jefatura TI)
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

        -- La funcionalidad debe pertenecer a la version de alcance mas
        -- reciente y APROBADA de la solicitud; si fue sustituida por una
        -- version posterior, no se permite programarla.
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
-- 3) sp_TI_Funcionalidades_RegresarABacklog
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
-- 4) sp_TI_Funcionalidades_ProgramacionVigente_ListadoPorSolicitud
--    (sin cambios de logica; se reinstala para que el script quede completo)
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

    SELECT FuncionalidadId, VersionAlcanceId, Codigo, Nombre, DiasEstimados, MesProgramado, AnioProgramado
    FROM dbo.TI_Funcionalidades
    WHERE VersionAlcanceId = @VersionAlcanceId AND Activa = 1
    ORDER BY Orden;
END
GO

-- ----------------------------------------------------------------------------
-- 5) sp_TI_Solicitudes_ResolverRevisionPrioridad (nuevo)
-- ----------------------------------------------------------------------------
CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_ResolverRevisionPrioridad
    @SolicitudId BIGINT,
    @EstadoResolucion NVARCHAR(20),   -- 'APROBADA' | 'RECHAZADA'
    @Motivo NVARCHAR(4000) = NULL,
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@EstadoResolucion IS NULL OR @EstadoResolucion NOT IN ('APROBADA', 'RECHAZADA'))
        BEGIN
            RAISERROR('El estado de resolucion debe ser APROBADA o RECHAZADA', 16, 1);
            RETURN;
        END
        IF (@EstadoResolucion = 'RECHAZADA' AND (@Motivo IS NULL OR LTRIM(RTRIM(@Motivo)) = ''))
        BEGIN
            RAISERROR('El motivo es obligatorio para rechazar la revision', 16, 1);
            RETURN;
        END

        DECLARE @SolicitanteUsuarioId INT, @EstadoActualId INT, @EstadoActualCodigo NVARCHAR(100);
        SELECT @SolicitanteUsuarioId = s.SolicitanteUsuarioId, @EstadoActualId = s.EstadoSolicitudId, @EstadoActualCodigo = es.Codigo
        FROM dbo.TI_Solicitudes s
        INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
        WHERE s.SolicitudId = @SolicitudId;

        IF (@EstadoActualId IS NULL)
        BEGIN
            RAISERROR('Solicitud no encontrada', 16, 1);
            RETURN;
        END

        -- Identidad real del solicitante -- nunca se confia en un rol para
        -- esta aprobacion, igual que sp_TI_AprobacionesAlcance_Registrar
        -- valida TipoAprobador = 'SOLICITANTE'.
        IF (@usuarioId <> @SolicitanteUsuarioId)
        BEGIN
            RAISERROR('Solo el usuario solicitante de esta solicitud puede resolver la revision de prioridad', 16, 1);
            RETURN;
        END

        IF (@EstadoActualCodigo <> 'EN_REVISION_PRIORIDAD')
        BEGIN
            RAISERROR('La solicitud no se encuentra en revision de prioridad', 16, 1);
            RETURN;
        END

        DECLARE @EstadoNuevoCodigo NVARCHAR(100) = CASE WHEN @EstadoResolucion = 'APROBADA' THEN 'TERMINADA' ELSE 'PENDIENTE_PRIORIZACION' END;
        DECLARE @EstadoNuevoId INT = (SELECT EstadoSolicitudId FROM dbo.TI_EstadosSolicitud WHERE Codigo = @EstadoNuevoCodigo);
        IF (@EstadoNuevoId IS NULL)
        BEGIN
            RAISERROR('El catalogo de estados no tiene registrado el codigo de destino; aplique primero el script de catalogo', 16, 1);
            RETURN;
        END

        UPDATE dbo.TI_Solicitudes
           SET EstadoSolicitudId = @EstadoNuevoId, FechaUltimaActualizacion = SYSUTCDATETIME()
         WHERE SolicitudId = @SolicitudId;

        INSERT INTO dbo.TI_SolicitudEstadosHistorial (SolicitudId, EstadoAnteriorId, EstadoNuevoId, UsuarioCambioId, FechaCambio, Motivo, Origen)
        VALUES (@SolicitudId, @EstadoActualId, @EstadoNuevoId, @usuarioId, SYSUTCDATETIME(), @Motivo, 'API');

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudId, @usuarioId, 'TI_Solicitudes', CAST(@SolicitudId AS NVARCHAR(200)),
                CASE WHEN @EstadoResolucion = 'APROBADA' THEN 'REVISION_PRIORIDAD_APROBADA' ELSE 'REVISION_PRIORIDAD_RECHAZADA' END,
                SYSUTCDATETIME(), @EstadoActualCodigo, @EstadoNuevoCodigo, 'API');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
