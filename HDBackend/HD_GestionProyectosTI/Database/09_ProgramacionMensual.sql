-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Programacion mensual por funcionalidad
-- (Bloque 2). Script preparado para REVISION Y EJECUCION MANUAL en el
-- entorno de desarrollo. No se ha ejecutado contra ningun servidor.
--
-- REQUIERE que 08_Migracion_ProgramacionMensual.sql ya se haya ejecutado
-- (agrega TI_Funcionalidades.MesProgramado/AnioProgramado y la tabla
-- dbo.TI_CambiosProgramacion).
--
-- Reglas de negocio de este bloque:
--   - REVISION del detalle de solicitud (ver instruccion del responsable):
--     la decision de prioridad GENERAL de la solicitud (TI_DecisionesPrioridad)
--     y la aprobacion dual de Planeacion sobre cambios de programacion
--     (TI_CambiosProgramacion/TI_AprobacionesPrioridad) quedan SIN USO en el
--     flujo nuevo. Jefatura de TI programa y reprograma cada funcionalidad
--     de forma DIRECTA (upsert), sin decision de solicitud y sin ninguna
--     aprobacion de Planeacion -- su acceso al modulo es solo de lectura.
--     Las tablas/SP anteriores NO se eliminan ni se modifican (quedan
--     disponibles para historico), simplemente dejan de invocarse aqui.
--   - sp_TI_Funcionalidades_ProgramarInicial ahora sirve tanto para la
--     primera programacion como para cualquier reprogramacion posterior
--     (ya no exige que la funcionalidad este sin programar ni depende de
--     ninguna decision de prioridad de la solicitud).
--   - Una funcionalidad sin MesProgramado/AnioProgramado permanece en el
--     backlog; no es obligatorio programar todas las funcionalidades de
--     una solicitud.
--   - TI_AuditoriaEventos se usa como bitacora complementaria (mismo
--     contrato ya usado en Fase 2A/2B).
-- ============================================================================

-- ----------------------------------------------------------------------------
-- PROGRAMACION (DIRECTA, SIN APROBACION) POR FUNCIONALIDAD
-- ----------------------------------------------------------------------------

-- Programa o reprograma (upsert) el mes/anio de una funcionalidad. Accion
-- directa de Jefatura de TI -- el controller la gatea por rol, este SP no
-- requiere ninguna aprobacion ni decision de prioridad de la solicitud.
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

        -- Revision por funcionalidad (ver 13_RevisionPorFuncionalidad.sql):
        -- mientras haya una revision PENDIENTE para esta funcionalidad,
        -- Jefatura de TI no puede programarla/reprogramarla -- debe esperar
        -- a que el solicitante la resuelva.
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

-- Devuelve una funcionalidad al backlog (limpia su programacion). Accion
-- directa de Jefatura de TI, sin aprobacion.
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

-- Incluye la revision/aprobacion vigente de cada funcionalidad (ver
-- 13_RevisionPorFuncionalidad.sql): se toma la fila mas reciente de
-- TI_CambiosProgramacion por funcionalidad y se compara su Mes/AnioPropuesto
-- (o MesAnterior/AnioAnteriorProgramado para el caso de rechazo) contra el
-- MesProgramado/AnioProgramado VIGENTE -- si no coincide (porque Jefatura de
-- TI reprogramo despues de esa resolucion), se ignora como no aplicable al
-- mes/anio actual, sin necesidad de ninguna columna ni estado adicional.
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
-- REVISION Y APROBACION POR FUNCIONALIDAD (ver 13_RevisionPorFuncionalidad.sql
-- para la explicacion completa). Se definen tambien aqui para que este
-- archivo quede como fuente canonica completa del bloque de programacion.
-- ----------------------------------------------------------------------------

-- Jefatura de TI envia a revision una funcionalidad ya programada. Crea una
-- fila PENDIENTE en TI_CambiosProgramacion con el mismo mes/anio vigente
-- (no es un cambio de programacion real, es una revision/aprobacion sobre
-- la programacion ya asignada) -- se reutiliza la tabla porque su esquema
-- (FuncionalidadId, SolicitadoPorUsuarioId, Motivo, EstadoCambio,
-- DecididoPorUsuarioId, FechaDecision, ComentarioResolucion) ya cubre
-- exactamente lo necesario, sin crear tablas ni columnas nuevas.
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

-- El usuario SOLICITANTE (identidad real, nunca un rol) aprueba o rechaza la
-- revision de una funcionalidad. Mismo patron atomico UPDATE...OUTPUT WHERE
-- EstadoCambio = 'PENDIENTE' que ya usa sp_TI_CambiosProgramacion_Resolver
-- para evitar la carrera de doble resolucion.
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

        -- Identidad real del solicitante -- nunca se confia en un rol,
        -- igual que sp_TI_Solicitudes_ResolverRevisionPrioridad.
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

        -- Estado general de la solicitud derivado del avance de aprobaciones
        -- por funcionalidad (nunca al revés: esto no sustituye la revision
        -- individual, solo refleja cuantas de las funcionalidades vigentes
        -- ya tienen aprobacion vigente). Se actualiza SOLO en el camino
        -- APROBADO -- un rechazo no mueve el estado general. No se agrega
        -- esta transicion a la whitelist de sp_TI_Solicitudes_CambiarEstado
        -- (igual que sp_TI_Solicitudes_ResolverRevisionPrioridad): es un
        -- efecto derivado de esta resolucion, no una accion que el cliente
        -- pueda disparar directamente por el endpoint generico.
        IF (@EstadoResolucion = 'APROBADO')
        BEGIN
            DECLARE @VersionVigenteId BIGINT = (
                SELECT TOP 1 VersionAlcanceId FROM dbo.TI_VersionesAlcance
                WHERE SolicitudId = @SolicitudId AND EstadoVersion = 'APROBADA'
                ORDER BY NumeroVersion DESC
            );

            IF (@VersionVigenteId IS NOT NULL)
            BEGIN
                DECLARE @TotalActivas INT = (
                    SELECT COUNT(*) FROM dbo.TI_Funcionalidades WHERE VersionAlcanceId = @VersionVigenteId AND Activa = 1
                );

                DECLARE @TotalAprobadas INT;
                ;WITH UltimoCambio AS (
                    SELECT c.*, ROW_NUMBER() OVER (PARTITION BY c.FuncionalidadId ORDER BY c.FechaSolicitud DESC) AS Orden_
                    FROM dbo.TI_CambiosProgramacion c
                    WHERE c.SolicitudId = @SolicitudId
                )
                SELECT @TotalAprobadas = COUNT(*)
                FROM dbo.TI_Funcionalidades f
                INNER JOIN UltimoCambio uc ON uc.FuncionalidadId = f.FuncionalidadId AND uc.Orden_ = 1
                WHERE f.VersionAlcanceId = @VersionVigenteId AND f.Activa = 1
                  AND uc.EstadoCambio = 'APROBADO' AND uc.MesPropuesto = f.MesProgramado AND uc.AnioPropuesto = f.AnioProgramado;

                DECLARE @EstadoActualSolId INT, @EstadoActualSolCodigo NVARCHAR(100);
                SELECT @EstadoActualSolId = s.EstadoSolicitudId, @EstadoActualSolCodigo = es.Codigo
                FROM dbo.TI_Solicitudes s
                INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
                WHERE s.SolicitudId = @SolicitudId;

                DECLARE @EstadoSolicitudDerivado NVARCHAR(100) = NULL;
                IF (@TotalActivas > 0 AND @TotalAprobadas = @TotalActivas)
                    SET @EstadoSolicitudDerivado = 'TERMINADA';
                ELSE IF (@TotalAprobadas >= 1)
                    SET @EstadoSolicitudDerivado = 'PRIORIZADA';

                IF (@EstadoSolicitudDerivado IS NOT NULL AND @EstadoActualSolCodigo <> @EstadoSolicitudDerivado)
                BEGIN
                    DECLARE @EstadoSolicitudDerivadoId INT = (SELECT EstadoSolicitudId FROM dbo.TI_EstadosSolicitud WHERE Codigo = @EstadoSolicitudDerivado);
                    IF (@EstadoSolicitudDerivadoId IS NOT NULL)
                    BEGIN
                        UPDATE dbo.TI_Solicitudes SET EstadoSolicitudId = @EstadoSolicitudDerivadoId, FechaUltimaActualizacion = SYSUTCDATETIME() WHERE SolicitudId = @SolicitudId;

                        INSERT INTO dbo.TI_SolicitudEstadosHistorial (SolicitudId, EstadoAnteriorId, EstadoNuevoId, UsuarioCambioId, FechaCambio, Motivo, Origen)
                        VALUES (@SolicitudId, @EstadoActualSolId, @EstadoSolicitudDerivadoId, @usuarioId, SYSUTCDATETIME(),
                                CASE WHEN @EstadoSolicitudDerivado = 'TERMINADA' THEN 'Todas las funcionalidades vigentes fueron aprobadas'
                                     ELSE CONCAT('Funcionalidades aprobadas: ', @TotalAprobadas, ' de ', @TotalActivas) END,
                                'API');

                        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
                        VALUES (@SolicitudId, @usuarioId, 'TI_Solicitudes', CAST(@SolicitudId AS NVARCHAR(200)), 'CAMBIAR_ESTADO', SYSUTCDATETIME(), @EstadoActualSolCodigo, @EstadoSolicitudDerivado, 'API');
                    END
                END
            END
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ----------------------------------------------------------------------------
-- CAMBIOS DE PROGRAMACION
-- ----------------------------------------------------------------------------

CREATE OR ALTER PROCEDURE dbo.sp_TI_CambiosProgramacion_Crear
    @FuncionalidadId BIGINT,
    @MesPropuesto TINYINT,
    @AnioPropuesto SMALLINT,
    @Motivo NVARCHAR(2000),
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF (@MesPropuesto IS NULL OR @MesPropuesto NOT BETWEEN 1 AND 12)
        BEGIN
            RAISERROR('El mes propuesto debe estar entre 1 y 12', 16, 1);
            RETURN;
        END
        IF (@AnioPropuesto IS NULL)
        BEGIN
            RAISERROR('El anio propuesto es obligatorio', 16, 1);
            RETURN;
        END
        IF (@Motivo IS NULL OR LTRIM(RTRIM(@Motivo)) = '')
        BEGIN
            RAISERROR('El motivo es obligatorio', 16, 1);
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
        IF (@MesActual IS NULL OR @AnioActual IS NULL)
        BEGIN
            RAISERROR('La funcionalidad todavia no tiene una programacion inicial; use sp_TI_Funcionalidades_ProgramarInicial', 16, 1);
            RETURN;
        END

        -- Igual que en la programacion inicial: solo se permite actuar
        -- sobre funcionalidades de la version de alcance vigente (la mas
        -- reciente APROBADA). Si la solicitud ya tiene un alcance nuevo
        -- que sustituyo a esta version, no se admite el cambio.
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

        IF (@MesPropuesto = @MesActual AND @AnioPropuesto = @AnioActual)
        BEGIN
            RAISERROR('El mes y anio propuestos son iguales a la programacion vigente; no hay cambio que registrar', 16, 1);
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM dbo.TI_CambiosProgramacion WHERE FuncionalidadId = @FuncionalidadId AND EstadoCambio = 'PENDIENTE')
        BEGIN
            RAISERROR('Ya existe un cambio de programacion pendiente para esta funcionalidad', 16, 1);
            RETURN;
        END

        INSERT INTO dbo.TI_CambiosProgramacion (FuncionalidadId, SolicitudId, MesAnteriorProgramado, AnioAnteriorProgramado, MesPropuesto, AnioPropuesto, SolicitadoPorUsuarioId, FechaSolicitud, Motivo, EstadoCambio)
        VALUES (@FuncionalidadId, @SolicitudId, @MesActual, @AnioActual, @MesPropuesto, @AnioPropuesto, @usuarioId, SYSUTCDATETIME(), @Motivo, 'PENDIENTE');

        DECLARE @CambioProgramacionId BIGINT = SCOPE_IDENTITY();

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudId, @usuarioId, 'TI_Funcionalidades', CAST(@FuncionalidadId AS NVARCHAR(200)), 'CAMBIO_PROGRAMACION_SOLICITADO', SYSUTCDATETIME(),
                CONCAT('Mes=', @MesActual, ' Anio=', @AnioActual), CONCAT('Mes=', @MesPropuesto, ' Anio=', @AnioPropuesto), 'API');

        COMMIT TRANSACTION;
        SELECT @CambioProgramacionId AS CambioProgramacionId;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Historial completo (pendientes + resueltos) de una solicitud, con el
-- codigo/nombre de la funcionalidad ya resuelto para mostrar en pantalla.
CREATE OR ALTER PROCEDURE dbo.sp_TI_CambiosProgramacion_ListadoPorSolicitud
    @SolicitudId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.CambioProgramacionId, c.FuncionalidadId, f.Codigo AS FuncionalidadCodigo, f.Nombre AS FuncionalidadNombre,
           c.SolicitudId, c.MesAnteriorProgramado, c.AnioAnteriorProgramado, c.MesPropuesto, c.AnioPropuesto,
           c.SolicitadoPorUsuarioId, c.FechaSolicitud, c.Motivo, c.EstadoCambio,
           c.DecididoPorUsuarioId, c.FechaDecision, c.ComentarioResolucion
    FROM dbo.TI_CambiosProgramacion c
    INNER JOIN dbo.TI_Funcionalidades f ON f.FuncionalidadId = c.FuncionalidadId
    WHERE c.SolicitudId = @SolicitudId
    ORDER BY c.FechaSolicitud DESC;
END
GO

-- TipoAprobador implicito: solo GERENCIA_PLANEACION puede resolver -- se
-- valida el rol real del usuario dentro del propio SP, nunca se confia en
-- el cliente.
CREATE OR ALTER PROCEDURE dbo.sp_TI_CambiosProgramacion_Resolver
    @CambioProgramacionId BIGINT,
    @EstadoResolucion NVARCHAR(20),
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

        IF NOT EXISTS (
            SELECT 1 FROM dbo.TI_UsuarioRoles ur INNER JOIN dbo.TI_Roles r ON r.RolId = ur.RolId
            WHERE ur.UsuarioId = @usuarioId AND ur.Activo = 1 AND r.Codigo = 'GERENCIA_PLANEACION'
        )
        BEGIN
            RAISERROR('El usuario no tiene el rol GERENCIA_PLANEACION', 16, 1);
            RETURN;
        END

        -- UPDATE atomico con OUTPUT: evita la carrera de doble resolucion
        -- (WHERE EstadoCambio = 'PENDIENTE' + @@ROWCOUNT) sin necesitar
        -- sp_getapplock, porque aqui solo hay UN lado que resuelve (no hay
        -- 2 aprobadores en carrera por lados distintos, a diferencia de
        -- TI_AprobacionesAlcance/TI_AprobacionesPrioridad).
        DECLARE @Resuelto TABLE (FuncionalidadId BIGINT, SolicitudId BIGINT, MesPropuesto TINYINT, AnioPropuesto SMALLINT, MesAnteriorProgramado TINYINT, AnioAnteriorProgramado SMALLINT);

        UPDATE dbo.TI_CambiosProgramacion
           SET EstadoCambio = @EstadoResolucion, DecididoPorUsuarioId = @usuarioId, FechaDecision = SYSUTCDATETIME(), ComentarioResolucion = @ComentarioResolucion
        OUTPUT inserted.FuncionalidadId, inserted.SolicitudId, inserted.MesPropuesto, inserted.AnioPropuesto, inserted.MesAnteriorProgramado, inserted.AnioAnteriorProgramado
        INTO @Resuelto
         WHERE CambioProgramacionId = @CambioProgramacionId AND EstadoCambio = 'PENDIENTE';

        IF (SELECT COUNT(*) FROM @Resuelto) = 0
        BEGIN
            RAISERROR('El cambio de programacion no existe o ya fue resuelto anteriormente', 16, 1);
            RETURN;
        END

        DECLARE @FuncionalidadId BIGINT, @SolicitudIdResuelto BIGINT, @MesPropuestoResuelto TINYINT, @AnioPropuestoResuelto SMALLINT, @MesAnterior TINYINT, @AnioAnterior SMALLINT;
        SELECT @FuncionalidadId = FuncionalidadId, @SolicitudIdResuelto = SolicitudId, @MesPropuestoResuelto = MesPropuesto, @AnioPropuestoResuelto = AnioPropuesto,
               @MesAnterior = MesAnteriorProgramado, @AnioAnterior = AnioAnteriorProgramado
        FROM @Resuelto;

        IF (@EstadoResolucion = 'APROBADO')
        BEGIN
            UPDATE dbo.TI_Funcionalidades
               SET MesProgramado = @MesPropuestoResuelto, AnioProgramado = @AnioPropuestoResuelto
             WHERE FuncionalidadId = @FuncionalidadId;
        END
        -- si se RECHAZA, la programacion vigente (MesAnterior/AnioAnterior)
        -- ya esta intacta en TI_Funcionalidades -- no se toca nada mas.

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudIdResuelto, @usuarioId, 'TI_Funcionalidades', CAST(@FuncionalidadId AS NVARCHAR(200)),
                CASE WHEN @EstadoResolucion = 'APROBADO' THEN 'CAMBIO_PROGRAMACION_APROBADO' ELSE 'CAMBIO_PROGRAMACION_RECHAZADO' END,
                SYSUTCDATETIME(), CONCAT('Mes=', @MesAnterior, ' Anio=', @AnioAnterior), CONCAT('Mes=', @MesPropuestoResuelto, ' Anio=', @AnioPropuestoResuelto), 'API');

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
