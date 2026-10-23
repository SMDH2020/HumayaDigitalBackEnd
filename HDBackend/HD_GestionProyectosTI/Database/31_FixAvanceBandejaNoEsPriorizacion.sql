-- ─────────────────────────────────────────────────────────────────────────
-- Bug reportado: en la Bandeja, la columna "Avance" marca 100% en
-- solicitudes con funcionalidades que todavia no se han terminado.
--
-- Causa: "Avance" se calculaba en el frontend como
-- FuncionalidadesAprobadas / FuncionalidadesTotal -- pero desde
-- 26_FixFuncionalidadesPriorizadasDefinicion.sql, FuncionalidadesAprobadas
-- ya NO significa "terminadas", significa "priorizadas" (con mes/anio
-- asignado, sin importar su avance real). Jefatura de TI programa TODAS las
-- funcionalidades de una solicitud (quedan "priorizadas") mucho antes de
-- que el desarrollo realmente avance -- eso hacia que "Avance" marcara 100%
-- desde el dia que se programan, sin relacion con el trabajo real.
--
-- Fix: se agrega una columna nueva, SumaDiasPorAvance = Sum(DiasEstimados *
-- AvanceManual) de las funcionalidades activas de la version vigente --
-- mismo criterio de ponderacion por dias que ya usa Backlog Mensual
-- (avancePonderado en BacklogMensualTIScreen.js). El frontend la divide
-- entre TotalDiasEstimados para sacar el % real. "Funcionalidades
-- Priorizadas" (X de Y) y "Priorizado" (meses) NO cambian -- siguen
-- representando priorizacion, no avance.
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
        agg.TotalDiasEstimados, agg.FuncionalidadesTotal, agg.SumaDiasPorAvance,
        aprob.FuncionalidadesAprobadas,
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
        SELECT SUM(f.DiasEstimados) AS TotalDiasEstimados, COUNT(*) AS FuncionalidadesTotal,
               SUM(f.DiasEstimados * ISNULL(f.AvanceManual, 0)) AS SumaDiasPorAvance
        FROM dbo.TI_Funcionalidades f
        WHERE f.VersionAlcanceId = vig.VersionAlcanceId AND f.Activa = 1
    ) agg
    OUTER APPLY (
        -- "Priorizada" = ya tiene Mes/Anio asignado, sin importar si ya se
        -- aprobo/termino -- misma definicion que PriorizacionFuncionalidadTI.js.
        -- Esto NO es avance real (ver SumaDiasPorAvance arriba).
        SELECT COUNT(*) AS FuncionalidadesAprobadas
        FROM dbo.TI_Funcionalidades f
        WHERE f.VersionAlcanceId = vig.VersionAlcanceId AND f.Activa = 1
          AND f.MesProgramado IS NOT NULL AND f.AnioProgramado IS NOT NULL
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
