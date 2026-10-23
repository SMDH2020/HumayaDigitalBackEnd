-- Diagnostico puntual: por que la funcionalidad de "Solicitudes Extras 2026
-- - Credito" (Terminada, Avance 0%) no cuenta como aprobada en el calculo
-- nuevo de sp_TI_Solicitudes_Listado. Pega el resultado de las dos
-- consultas para poder comparar contra lo que espera el calculo.

DECLARE @SolicitudId BIGINT = (
    SELECT TOP 1 SolicitudId FROM dbo.TI_Solicitudes
    WHERE Titulo = 'Solicitudes Extras 2026 - Credito'
);

-- 1) La(s) funcionalidad(es) de esa solicitud y su programacion actual.
SELECT f.FuncionalidadId, f.Nombre, f.Activa, f.AvanceManual,
       f.MesProgramado, f.AnioProgramado, f.VersionAlcanceId
FROM dbo.TI_Funcionalidades f
INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId
WHERE v.SolicitudId = @SolicitudId;

-- 2) TODO el historial de TI_CambiosProgramacion de esas funcionalidades
-- (no solo el ultimo) -- para ver si el ultimo registro por fecha es
-- realmente el aprobado, o si hay un registro mas reciente (ej. una
-- reprogramacion) que invalido esa aprobacion.
SELECT c.CambioProgramacionId, c.FuncionalidadId, c.EstadoCambio,
       c.MesAnteriorProgramado, c.AnioAnteriorProgramado,
       c.MesPropuesto, c.AnioPropuesto,
       c.FechaSolicitud, c.FechaDecision
FROM dbo.TI_CambiosProgramacion c
WHERE c.SolicitudId = @SolicitudId
ORDER BY c.FuncionalidadId, c.FechaSolicitud DESC;
