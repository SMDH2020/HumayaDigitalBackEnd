-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- El estado general de la solicitud
-- ahora refleja el avance de las aprobaciones por funcionalidad. Script
-- preparado para REVISION Y EJECUCION MANUAL. No se ha ejecutado contra
-- ningun servidor.
--
-- Motivo: aprobar funcionalidades individualmente no movia el estado
-- general de la solicitud -- aunque se aprobaran todas, la solicitud nunca
-- llegaba a TERMINADA. Se agrega la regla:
--   - En cuanto se aprueba AL MENOS UNA funcionalidad vigente -> PRIORIZADA
--     (el frontend calcula y muestra "X de Y" a partir de las mismas
--     funcionalidades que ya lista PriorizacionTI/ProgramacionVigente, sin
--     necesidad de ningun dato nuevo del backend).
--   - En cuanto se aprueban TODAS las funcionalidades activas de la version
--     de alcance vigente -> TERMINADA.
-- Un RECHAZO no mueve el estado general (solo afecta a esa funcionalidad).
--
-- Por que no se agrega esta transicion a la whitelist de
-- sp_TI_Solicitudes_CambiarEstado: es un efecto DERIVADO de la resolucion
-- de una revision por funcionalidad (sp_TI_Funcionalidades_ResolverRevision,
-- que ya valida la identidad real del solicitante), no una accion que el
-- cliente pueda disparar directamente -- mismo criterio ya usado para
-- sp_TI_Solicitudes_ResolverRevisionPrioridad.
--
-- Contenido: cuerpo completo de sp_TI_Funcionalidades_ResolverRevision
-- (CREATE OR ALTER PROCEDURE requiere el cuerpo completo). La unica
-- diferencia frente a la version ya instalada es el bloque nuevo que corre
-- SOLO cuando @EstadoResolucion = 'APROBADO', despues de registrar la
-- auditoria de la funcionalidad: cuenta funcionalidades activas de la
-- version vigente vs. cuantas tienen una aprobacion vigente (misma
-- comparacion Mes/AnioPropuesto contra el MesProgramado/AnioProgramado
-- actual que ya usa sp_TI_Funcionalidades_ProgramacionVigente_ListadoPorSolicitud
-- para calcular RevisionEstado), y si corresponde cambia el estado de
-- TI_Solicitudes con su propio historial y auditoria.
--
-- No se crea ninguna tabla ni columna. No se modifica ningun otro
-- procedimiento.
-- ============================================================================

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
