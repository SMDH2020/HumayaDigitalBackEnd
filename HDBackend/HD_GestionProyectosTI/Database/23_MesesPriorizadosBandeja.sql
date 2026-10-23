-- ─────────────────────────────────────────────────────────────────────────
-- Columna "Priorizado" de la Bandeja de Solicitudes TI: todos los meses en
-- los que hay al menos una funcionalidad priorizada (MesProgramado/
-- AnioProgramado) dentro de la version de alcance vigente, no solo la mas
-- nueva -- como etiquetas (ej. "Agosto-2026, Octubre-2026, Noviembre-2026").
-- ─────────────────────────────────────────────────────────────────────────

CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_Listado
    @usuarioId      INT,
    @verTodo        BIT,
    @estadoCodigo   NVARCHAR(100)  = NULL,
    @tipoCodigo     NVARCHAR(80)   = NULL,
    @fechaInicio    DATE           = NULL,
    @fechaFin       DATE           = NULL,
    @areas          NVARCHAR(MAX)  = NULL  -- areas solicitantes separadas por coma; NULL o vacio = todas
AS
BEGIN
    SET NOCOUNT ON;

    -- Nombre real del solicitante\creador (ver 17_NombresRealesUsuarios.sql):
    -- UsuarioId = AppMH.dbo.Empleados.IDEmpleado, mismo servidor SQL.
    SELECT
        s.SolicitudId, s.Folio, s.Titulo, s.DescripcionNecesidad, s.ProblemaActual, s.BeneficioEsperado,
        s.TipoSolicitudId, ts.Codigo AS TipoSolicitudCodigo, ts.Nombre AS TipoSolicitud,
        s.EstadoSolicitudId, es.Codigo AS EstadoCodigo, es.Nombre AS EstadoSolicitud, es.Etapa,
        s.PrioridadId, p.Codigo AS PrioridadCodigo, p.Nombre AS Prioridad,
        s.SolicitanteUsuarioId, s.CreadaPorUsuarioId, s.AreaSolicitante, s.EsConfidencial,
        s.MotivoRechazo, s.MotivoCancelacion,
        s.FechaCreacion, s.FechaEnvio, s.FechaUltimaActualizacion, s.FechaCierre,
        CONCAT(empSol.Nombre, ' ', empSol.ApellidoPaterno, ' ', empSol.ApellidoMaterno) AS SolicitanteNombre,
        CONCAT(empCrea.Nombre, ' ', empCrea.ApellidoPaterno, ' ', empCrea.ApellidoMaterno) AS CreadaPorNombre,
        empSol.Correo AS SolicitanteCorreo,
        agg.TotalDiasEstimados, agg.FuncionalidadesTotal, aprob.FuncionalidadesAprobadas,
        meses.MesesPriorizados
    FROM dbo.TI_Solicitudes s
    INNER JOIN dbo.TI_TiposSolicitud ts ON ts.TipoSolicitudId = s.TipoSolicitudId
    INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
    LEFT JOIN dbo.TI_Prioridades p ON p.PrioridadId = s.PrioridadId
    LEFT JOIN AppMH.dbo.Empleados empSol ON empSol.IDEmpleado = s.SolicitanteUsuarioId AND empSol.Estatus = 1
    LEFT JOIN AppMH.dbo.Empleados empCrea ON empCrea.IDEmpleado = s.CreadaPorUsuarioId AND empCrea.Estatus = 1
    OUTER APPLY (
        SELECT TOP 1 va.VersionAlcanceId
        FROM dbo.TI_VersionesAlcance va
        WHERE va.SolicitudId = s.SolicitudId AND va.EstadoVersion = 'APROBADA'
        ORDER BY va.NumeroVersion DESC
    ) vig
    OUTER APPLY (
        SELECT SUM(f.DiasEstimados) AS TotalDiasEstimados, COUNT(*) AS FuncionalidadesTotal
        FROM dbo.TI_Funcionalidades f
        WHERE f.VersionAlcanceId = vig.VersionAlcanceId AND f.Activa = 1
    ) agg
    OUTER APPLY (
        SELECT COUNT(*) AS FuncionalidadesAprobadas
        FROM dbo.TI_Funcionalidades f
        LEFT JOIN (
            SELECT c.*, ROW_NUMBER() OVER (PARTITION BY c.FuncionalidadId ORDER BY c.FechaSolicitud DESC) AS Orden_
            FROM dbo.TI_CambiosProgramacion c
            WHERE c.SolicitudId = s.SolicitudId
        ) uc ON uc.FuncionalidadId = f.FuncionalidadId AND uc.Orden_ = 1
        WHERE f.VersionAlcanceId = vig.VersionAlcanceId AND f.Activa = 1
          AND uc.EstadoCambio = 'APROBADO' AND uc.MesPropuesto = f.MesProgramado AND uc.AnioPropuesto = f.AnioProgramado
    ) aprob
    OUTER APPLY (
        SELECT STRING_AGG(CONCAT(mesesDistintos.AnioProgramado, '-', mesesDistintos.MesProgramado), ',')
                 WITHIN GROUP (ORDER BY mesesDistintos.AnioProgramado, mesesDistintos.MesProgramado) AS MesesPriorizados
        FROM (
            SELECT DISTINCT f.AnioProgramado, f.MesProgramado
            FROM dbo.TI_Funcionalidades f
            WHERE f.VersionAlcanceId = vig.VersionAlcanceId AND f.Activa = 1
              AND f.MesProgramado IS NOT NULL AND f.AnioProgramado IS NOT NULL
        ) mesesDistintos
    ) meses
    WHERE (@estadoCodigo IS NULL OR es.Codigo = @estadoCodigo)
      AND (@tipoCodigo IS NULL OR ts.Codigo = @tipoCodigo)
      AND (@fechaInicio IS NULL OR CAST(s.FechaCreacion AS DATE) >= @fechaInicio)
      AND (@fechaFin IS NULL OR CAST(s.FechaCreacion AS DATE) <= @fechaFin)
      AND (@areas IS NULL OR @areas = '' OR s.AreaSolicitante IN (SELECT value FROM STRING_SPLIT(@areas, ',')))
      AND (
            @verTodo = 1
            OR s.SolicitanteUsuarioId = @usuarioId
            OR s.CreadaPorUsuarioId = @usuarioId
            OR EXISTS (
                SELECT 1 FROM dbo.TI_SolicitudParticipantes sp
                WHERE sp.SolicitudId = s.SolicitudId AND sp.UsuarioId = @usuarioId AND sp.Activo = 1
            )
          )
    ORDER BY s.FechaCreacion DESC;
END
GO
