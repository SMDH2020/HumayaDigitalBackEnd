-- ─────────────────────────────────────────────────────────────────────────
-- Bug reportado: una funcionalidad con su revision de priorizacion ya
-- APROBADA por el solicitante no sumaba en la columna "Avance" de la
-- Bandeja (ver 31_FixAvanceBandejaNoEsPriorizacion.sql, que calcula el
-- avance a partir de AvanceManual).
--
-- Causa: aprobar la revision (sp_TI_Funcionalidades_ResolverRevision) solo
-- cambiaba TI_CambiosProgramacion.EstadoCambio a 'APROBADO' -- nunca tocaba
-- TI_Funcionalidades.AvanceManual. Regla de negocio aclarada por el
-- usuario: cuando Jefatura de TI manda una funcionalidad a revision y el
-- SOLICITANTE la aprueba, esa funcionalidad se considera TERMINADA de ahi
-- en adelante (AvanceManual = 100), y eso es lo que debe sumar en el avance
-- general de la solicitud.
--
-- Fix: mismo cuerpo que 30_FixEstadoSolicitudDesactualizado.sql para
-- sp_TI_Funcionalidades_ResolverRevision, agregando
-- "UPDATE TI_Funcionalidades SET AvanceManual = 100" dentro del camino
-- APROBADO, antes de recalcular el estado derivado de la solicitud (ese
-- recalculo ya usaba el EstadoCambio aprobado para TERMINADA/EN_BACKLOG;
-- ahora tambien queda reflejado en el avance ponderado por dias de la
-- Bandeja). Un RECHAZO no toca AvanceManual -- solo afecta el mes/anio
-- propuesto, no revierte trabajo ya hecho.
-- ─────────────────────────────────────────────────────────────────────────

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

        -- Aprobar la revision = dar por terminada la funcionalidad. Esto es
        -- lo que hace que "Avance" (Bandeja) y el avance ponderado de
        -- Backlog Mensual reflejen esta aprobacion -- antes solo quedaba
        -- registrada en TI_CambiosProgramacion, sin tocar el avance real.
        IF (@EstadoResolucion = 'APROBADO')
        BEGIN
            UPDATE dbo.TI_Funcionalidades SET AvanceManual = 100 WHERE FuncionalidadId = @FuncionalidadId;
        END

        INSERT INTO dbo.TI_AuditoriaEventos (SolicitudId, UsuarioId, Entidad, EntidadId, Accion, FechaEvento, ValorAnterior, ValorNuevo, Origen)
        VALUES (@SolicitudId, @usuarioId, 'TI_Funcionalidades', CAST(@FuncionalidadId AS NVARCHAR(200)),
                CASE WHEN @EstadoResolucion = 'APROBADO' THEN 'FUNCIONALIDAD_REVISION_APROBADA' ELSE 'FUNCIONALIDAD_REVISION_RECHAZADA' END,
                SYSUTCDATETIME(), NULL, @ComentarioResolucion, 'API');

        EXEC dbo.sp_TI_Solicitudes_RecalcularEstadoDerivado @SolicitudId = @SolicitudId, @usuarioId = @usuarioId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- ─────────────────────────────────────────────────────────────────────────
-- Backfill (ejecutar UNA SOLA VEZ): el fix de arriba solo aplica hacia
-- adelante -- las funcionalidades que YA tienen su revision vigente
-- aprobada (aprobadas antes de este script) no se actualizan solas. Se
-- marcan aqui con el mismo criterio de "aprobacion vigente" que usa
-- sp_TI_Solicitudes_RecalcularEstadoDerivado (ultimo cambio de esa
-- funcionalidad, EstadoCambio='APROBADO', y su mes/anio propuesto coincide
-- con el mes/anio programado ACTUAL -- si se reprogramo despues de
-- aprobarse, esa aprobacion ya no aplica y no se toca).
-- ─────────────────────────────────────────────────────────────────────────
;WITH UltimoCambio AS (
    SELECT c.*, ROW_NUMBER() OVER (PARTITION BY c.FuncionalidadId ORDER BY c.FechaSolicitud DESC) AS Orden_
    FROM dbo.TI_CambiosProgramacion c
)
UPDATE f
   SET AvanceManual = 100
FROM dbo.TI_Funcionalidades f
INNER JOIN UltimoCambio uc ON uc.FuncionalidadId = f.FuncionalidadId AND uc.Orden_ = 1
WHERE f.Activa = 1
  AND uc.EstadoCambio = 'APROBADO' AND uc.MesPropuesto = f.MesProgramado AND uc.AnioPropuesto = f.AnioProgramado
  AND (f.AvanceManual IS NULL OR f.AvanceManual <> 100);
