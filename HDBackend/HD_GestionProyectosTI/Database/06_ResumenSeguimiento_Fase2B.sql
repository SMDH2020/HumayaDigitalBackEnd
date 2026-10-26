-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Resumen de seguimiento (solo lectura)
-- Script preparado para REVISION Y EJECUCION MANUAL. No se ha ejecutado
-- contra ningun servidor.
--
-- Motivo: no existe ningun SP que devuelva, para una SolicitudId, cual es
-- su version de alcance / estimacion / evaluacion de prioridad / decision
-- de prioridad VIGENTE (la mas reciente por NumeroVersion/NumeroEvaluacion/
-- NumeroDecision). Sin esto, la pantalla de Seguimiento del frontend solo
-- podria reconstruirse mientras los Id quedaran en memoria de la sesion
-- activa de React (se perderian al recargar la pagina). Es de solo lectura,
-- no agrega ninguna regla de negocio nueva ni toca ninguna tabla.
--
-- Tambien devuelve el detalle de las aprobaciones de la version de alcance
-- y de la decision de prioridad vigentes, para que las pantallas de
-- aprobacion muestren que lado ya esta resuelto sin tener que adivinarlo.
-- ============================================================================

CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_ResumenSeguimiento
    @SolicitudId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.TI_Solicitudes WHERE SolicitudId = @SolicitudId)
    BEGIN
        RAISERROR('Solicitud no encontrada', 16, 1);
        RETURN;
    END

    -- Cadena de vigencia: la estimacion y la evaluacion mostradas deben
    -- pertenecer a la MISMA version de alcance vigente (no a la solicitud
    -- en general -- una version anterior pudo tener su propia estimacion/
    -- evaluacion, que ya no corresponde a la version actual). La decision
    -- mostrada debe pertenecer a esa MISMA evaluacion (no solo a la
    -- solicitud), igual que valida sp_TI_DecisionesPrioridad_Crear.
    DECLARE @VersionAlcanceId BIGINT = (
        SELECT TOP 1 VersionAlcanceId FROM dbo.TI_VersionesAlcance
        WHERE SolicitudId = @SolicitudId ORDER BY NumeroVersion DESC
    );
    DECLARE @EstimacionId BIGINT = (
        SELECT TOP 1 EstimacionId FROM dbo.TI_Estimaciones
        WHERE SolicitudId = @SolicitudId AND VersionAlcanceId = @VersionAlcanceId
        ORDER BY NumeroVersion DESC
    );
    DECLARE @EvaluacionPrioridadId BIGINT = (
        SELECT TOP 1 EvaluacionPrioridadId FROM dbo.TI_EvaluacionesPrioridad
        WHERE SolicitudId = @SolicitudId AND VersionAlcanceId = @VersionAlcanceId
        ORDER BY NumeroEvaluacion DESC
    );
    -- Bloque 2: la decision de prioridad ya no depende de una evaluacion
    -- (sp_TI_DecisionesPrioridad_Crear la hace opcional); se recupera la
    -- mas reciente de la solicitud directamente por SolicitudId. Filtrar
    -- antes por EvaluacionPrioridadId era ademas un NULL-trap: cuando no
    -- existe evaluacion, tanto @EvaluacionPrioridadId como la columna de la
    -- decision son NULL, y "NULL = NULL" nunca es TRUE en SQL Server, asi
    -- que la decision jamas se encontraba.
    --
    -- Decision VIGENTE (la que gobierna la prioridad/programacion mostrada):
    -- la ultima que llego a CONFIRMADA. Mientras exista una repriorizacion
    -- PENDIENTE, esta sigue siendo la vigente -- la pendiente se expone por
    -- separado (@DecisionPendienteId, result sets 4 y 5) sin sustituirla,
    -- igual que TI_CambiosProgramacion deja intacta la programacion vigente
    -- mientras un cambio de mes esta PENDIENTE.
    DECLARE @DecisionPrioridadId BIGINT = (
        SELECT TOP 1 DecisionPrioridadId FROM dbo.TI_DecisionesPrioridad
        WHERE SolicitudId = @SolicitudId AND EstadoDecision = 'CONFIRMADA'
        ORDER BY NumeroDecision DESC
    );
    -- Repriorizacion pendiente de autorizacion de Planeacion (a lo sumo 1,
    -- reforzado por sp_TI_DecisionesPrioridad_Crear).
    DECLARE @DecisionPendienteId BIGINT = (
        SELECT TOP 1 DecisionPrioridadId FROM dbo.TI_DecisionesPrioridad
        WHERE SolicitudId = @SolicitudId AND EstadoDecision = 'PENDIENTE'
        ORDER BY NumeroDecision DESC
    );

    -- Result set 1: resumen de una sola fila.
    SELECT
        s.SolicitudId,
        v.VersionAlcanceId, v.NumeroVersion AS VersionAlcanceNumero, v.EstadoVersion,
        e.EstimacionId, e.NumeroVersion AS EstimacionNumero, e.EstadoEstimacion, e.HorasTotal,
        ev.EvaluacionPrioridadId, ev.NumeroEvaluacion, ev.EstadoEvaluacion, ev.PuntajeTotal,
        d.DecisionPrioridadId, d.NumeroDecision, d.TipoDecision, d.EstadoDecision, d.PrioridadId AS DecisionPrioridadIdSeleccionada, d.PosicionBacklog,
        pd.DecisionPrioridadId AS RepriorizacionPendienteId, pd.NumeroDecision AS RepriorizacionNumeroDecision,
        pd.TipoDecision AS RepriorizacionTipoDecision, pd.PrioridadId AS RepriorizacionPrioridadId,
        pd.PosicionBacklog AS RepriorizacionPosicionBacklog, pd.Motivo AS RepriorizacionMotivo
    FROM dbo.TI_Solicitudes s
    LEFT JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = @VersionAlcanceId
    LEFT JOIN dbo.TI_Estimaciones e ON e.EstimacionId = @EstimacionId
    LEFT JOIN dbo.TI_EvaluacionesPrioridad ev ON ev.EvaluacionPrioridadId = @EvaluacionPrioridadId
    LEFT JOIN dbo.TI_DecisionesPrioridad d ON d.DecisionPrioridadId = @DecisionPrioridadId
    LEFT JOIN dbo.TI_DecisionesPrioridad pd ON pd.DecisionPrioridadId = @DecisionPendienteId
    WHERE s.SolicitudId = @SolicitudId;

    -- Result set 2: aprobaciones de la version de alcance vigente (vacio si no hay ninguna).
    SELECT TipoAprobador, EstadoAprobacion, AprobadorUsuarioId, FechaSolicitud, FechaDecision, Comentario
    FROM dbo.TI_AprobacionesAlcance
    WHERE VersionAlcanceId = @VersionAlcanceId;

    -- Result set 3: aprobaciones de la decision de prioridad vigente (vacio si no hay ninguna).
    SELECT TipoAprobador, EstadoAprobacion, AprobadorUsuarioId, FechaSolicitud, FechaDecision, Comentario
    FROM dbo.TI_AprobacionesPrioridad
    WHERE DecisionPrioridadId = @DecisionPrioridadId;

    -- Result set 4: aprobaciones de la repriorizacion pendiente (vacio si no hay ninguna pendiente).
    SELECT TipoAprobador, EstadoAprobacion, AprobadorUsuarioId, FechaSolicitud, FechaDecision, Comentario
    FROM dbo.TI_AprobacionesPrioridad
    WHERE DecisionPrioridadId = @DecisionPendienteId;
END
GO
