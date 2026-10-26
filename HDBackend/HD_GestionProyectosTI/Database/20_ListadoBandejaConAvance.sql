-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- sp_TI_Solicitudes_Listado agrega 3
-- columnas calculadas para la bandeja de solicitudes:
--   - TotalDiasEstimados: suma de DiasEstimados de las funcionalidades
--     activas de la version de alcance APROBADA vigente (NULL si la
--     solicitud no tiene ninguna version aprobada todavia).
--   - FuncionalidadesTotal / FuncionalidadesAprobadas: mismo conteo que ya
--     usa el frontend en PriorizacionFuncionalidadTI.js ("Priorizada X de
--     Y") -- una funcionalidad cuenta como aprobada cuando su ultimo
--     TI_CambiosProgramacion esta EstadoCambio = 'APROBADO' y coincide con
--     el Mes/AnioProgramado vigente de la funcionalidad (mismo criterio que
--     sp_TI_Funcionalidades_ProgramacionVigente_ListadoPorSolicitud, ver
--     13_RevisionPorFuncionalidad.sql). El % de avance en el frontend se
--     calcula como FuncionalidadesAprobadas / FuncionalidadesTotal.
--
-- Script preparado para REVISION Y EJECUCION MANUAL. No se ha ejecutado
-- contra ningun servidor.
-- ============================================================================

CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_Listado
    @usuarioId      INT,
    @verTodo        BIT,
    @estadoCodigo   NVARCHAR(100) = NULL,
    @tipoCodigo     NVARCHAR(80)  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Nombre real del solicitante/creador (ver 17_NombresRealesUsuarios.sql):
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
        agg.TotalDiasEstimados, agg.FuncionalidadesTotal, aprob.FuncionalidadesAprobadas
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
    WHERE (@estadoCodigo IS NULL OR es.Codigo = @estadoCodigo)
      AND (@tipoCodigo IS NULL OR ts.Codigo = @tipoCodigo)
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
