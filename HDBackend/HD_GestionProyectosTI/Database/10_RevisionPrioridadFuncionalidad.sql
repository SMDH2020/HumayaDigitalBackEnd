-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Revision y aprobacion de la
-- priorizacion por funcionalidad (cierre del detalle de solicitud).
-- Script preparado para REVISION Y EJECUCION MANUAL. No se ha ejecutado
-- contra ningun servidor.
--
-- REQUIERE que el catalogo dbo.TI_EstadosSolicitud ya tenga las filas
-- 'EN_REVISION_PRIORIDAD' y 'TERMINADA' (script de catalogo pendiente de
-- autorizacion -- ver inspeccion de columnas solicitada antes de este
-- script). NO EJECUTAR este archivo hasta que ese INSERT de catalogo este
-- aplicado, o sp_TI_Solicitudes_CambiarEstado fallara al no encontrar
-- @EstadoNuevoId para esos codigos.
--
-- Regla de negocio (definida por el responsable):
--   - Jefatura de TI prioriza cada funcionalidad (ver 09_ProgramacionMensual.sql,
--     ya sin decision de solicitud ni aprobacion de Planeacion).
--   - Cuando Jefatura considera que la priorizacion esta lista, envia la
--     solicitud a EN_REVISION_PRIORIDAD -- esto NO requiere un SP nuevo: se
--     hace con el endpoint generico SolicitudesTI/CambiarEstado, ya
--     autorizado para su rol (ver whitelist ampliada en
--     03_Actualizacion_CambiarEstado_Fase2B.sql).
--   - El usuario SOLICITANTE (identidad real, no solo rol) resuelve esa
--     revision con el SP de este archivo: aprobar -> TERMINADA (cierre del
--     proceso de revision/autorizacion, NO implica que el desarrollo ya
--     se haya hecho); rechazar -> regresa a PENDIENTE_PRIORIZACION para que
--     Jefatura reajuste.
--   - IMPORTANTE (seguridad): EN_REVISION_PRIORIDAD -> TERMINADA y
--     EN_REVISION_PRIORIDAD -> PENDIENTE_PRIORIZACION NO se agregan a la
--     whitelist de sp_TI_Solicitudes_CambiarEstado. Si se agregaran ahi,
--     Jefatura de TI podria alcanzar TERMINADA directo por el endpoint
--     generico (gateado solo por ROL, no por identidad), saltandose la
--     aprobacion real del solicitante. Por eso este SP hace su propia
--     transaccion/UPDATE/historial en vez de llamar a
--     sp_TI_Solicitudes_CambiarEstado para esta transicion puntual.
--   - Transaccional, con el mismo patron SET XACT_ABORT ON + TRY/CATCH +
--     ROLLBACK ya usado en todo el modulo; escribe en
--     TI_SolicitudEstadosHistorial y TI_AuditoriaEventos igual que el
--     mecanismo central, para no perder trazabilidad.
-- ============================================================================

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
