-- Ajuste final al calculo de "Avance" de la Bandeja (ver
-- 33_AvanceDesdeAprobacionSinTocarAvanceManual.sql): el usuario confirmo
-- que AvanceManual (el % que Jefatura edita a mano en Backlog Mensual) no
-- debe participar para NADA en este calculo -- es puramente binario por
-- funcionalidad: 100% si su revision vigente esta APROBADA por el
-- solicitante, 0% en cualquier otro caso (sin aprobar todavia, rechazada,
-- o sin siquiera haberse enviado a revision).
--
-- Unico cambio sobre 33: el ELSE del CASE pasa de ISNULL(f.AvanceManual, 0)
-- a un 0 literal.

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
        -- EfectivoAvance por funcionalidad = 100 si su revision vigente
        -- esta APROBADA por el solicitante (terminada), 0 en cualquier
        -- otro caso -- AvanceManual NO participa aqui (es un campo
        -- distinto, de uso exclusivo de Backlog Mensual).
        SELECT
            SUM(f.DiasEstimados) AS TotalDiasEstimados,
            COUNT(*) AS FuncionalidadesTotal,
            SUM(f.DiasEstimados * (
                CASE WHEN uc.EstadoCambio = 'APROBADO' AND uc.MesPropuesto = f.MesProgramado AND uc.AnioPropuesto = f.AnioProgramado
                     THEN 100
                     ELSE 0
                END
            )) AS SumaDiasPorAvance
        FROM dbo.TI_Funcionalidades f
        OUTER APPLY (
            SELECT TOP 1 c.EstadoCambio, c.MesPropuesto, c.AnioPropuesto
            FROM dbo.TI_CambiosProgramacion c
            WHERE c.FuncionalidadId = f.FuncionalidadId
            ORDER BY c.FechaSolicitud DESC
        ) uc
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
