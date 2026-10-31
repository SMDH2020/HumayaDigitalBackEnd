-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Nombres reales de usuario (en vez de
-- solo el UsuarioId numerico) en listado/detalle/historial de solicitudes.
-- Script preparado para REVISION Y EJECUCION MANUAL. No se ha ejecutado
-- contra ningun servidor.
--
-- Fuente de nombre/correo: AppMH.dbo.Empleados (misma tabla/columnas ya
-- usadas en el resto del sistema para esto, ver
-- HD_Notifications/SegumientoActividades/CorreosSeguimientoAct.cs:
--   SELECT IDEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, Correo
--   FROM AppMH.dbo.Empleados WHERE Estatus = 1
--
-- SUPUESTO (no verificable desde el codigo, se solicita confirmar): que
-- SolicitanteUsuarioId / CreadaPorUsuarioId / UsuarioCambioId / UsuarioId en
-- todas las tablas TI_* correspondan exactamente a AppMH.dbo.Empleados.IDEmpleado
-- -- es el mismo criterio que ya usa el modulo de Seguimiento de Actividades
-- (CorreosSeguimientoAct.ObtenerContactoEmpleado) para sus propios ids de
-- usuario, y por eso se replica aqui. Si en este modulo los ids de usuario
-- vinieran de otra tabla, los LEFT JOIN de abajo simplemente no encontrarian
-- coincidencia y el nombre/correo saldrian NULL (no truena nada, pero avisar
-- si se observa eso en pruebas).
--
-- GestionProyectosTI y AppMH estan en el MISMO servidor SQL (confirmado en
-- appsettings.json), por eso el cruce se hace directo con el nombre de 3/4
-- partes (AppMH.dbo.Empleados) usando la misma conexion de
-- HumayaDigital_GestionProyectosTI -- no se necesita otra cadena de conexion.
--
-- No se crea ninguna tabla ni columna, solo se agregan LEFT JOIN + columnas
-- calculadas de salida a 3 procedimientos ya existentes. Se usa CONCAT()
-- (nunca regresa NULL por un campo NULL, a diferencia de usar "+") para que
-- un ApellidoMaterno vacio no deje el nombre completo en NULL.
-- ============================================================================

CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_Listado
    @usuarioId      INT,
    @verTodo        BIT,
    @estadoCodigo   NVARCHAR(100) = NULL,
    @tipoCodigo     NVARCHAR(80)  = NULL
AS
BEGIN
    SET NOCOUNT ON;

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
        empSol.Correo AS SolicitanteCorreo
    FROM dbo.TI_Solicitudes s
    INNER JOIN dbo.TI_TiposSolicitud ts ON ts.TipoSolicitudId = s.TipoSolicitudId
    INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
    LEFT JOIN dbo.TI_Prioridades p ON p.PrioridadId = s.PrioridadId
    LEFT JOIN AppMH.dbo.Empleados empSol ON empSol.IDEmpleado = s.SolicitanteUsuarioId AND empSol.Estatus = 1
    LEFT JOIN AppMH.dbo.Empleados empCrea ON empCrea.IDEmpleado = s.CreadaPorUsuarioId AND empCrea.Estatus = 1
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

CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_Obtener
    @solicitudId BIGINT
AS
BEGIN
    SET NOCOUNT ON;

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
        empSol.Correo AS SolicitanteCorreo
    FROM dbo.TI_Solicitudes s
    INNER JOIN dbo.TI_TiposSolicitud ts ON ts.TipoSolicitudId = s.TipoSolicitudId
    INNER JOIN dbo.TI_EstadosSolicitud es ON es.EstadoSolicitudId = s.EstadoSolicitudId
    LEFT JOIN dbo.TI_Prioridades p ON p.PrioridadId = s.PrioridadId
    LEFT JOIN AppMH.dbo.Empleados empSol ON empSol.IDEmpleado = s.SolicitanteUsuarioId AND empSol.Estatus = 1
    LEFT JOIN AppMH.dbo.Empleados empCrea ON empCrea.IDEmpleado = s.CreadaPorUsuarioId AND empCrea.Estatus = 1
    WHERE s.SolicitudId = @solicitudId;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TI_Solicitudes_Historial
    @solicitudId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        h.HistorialEstadoId, h.SolicitudId,
        h.EstadoAnteriorId, ea.Codigo AS EstadoAnteriorCodigo, ea.Nombre AS EstadoAnteriorNombre,
        h.EstadoNuevoId, en.Codigo AS EstadoNuevoCodigo, en.Nombre AS EstadoNuevoNombre,
        h.UsuarioCambioId, h.FechaCambio, h.Motivo, h.Origen,
        CONCAT(emp.Nombre, ' ', emp.ApellidoPaterno, ' ', emp.ApellidoMaterno) AS UsuarioCambioNombre
    FROM dbo.TI_SolicitudEstadosHistorial h
    LEFT JOIN dbo.TI_EstadosSolicitud ea ON ea.EstadoSolicitudId = h.EstadoAnteriorId
    INNER JOIN dbo.TI_EstadosSolicitud en ON en.EstadoSolicitudId = h.EstadoNuevoId
    LEFT JOIN AppMH.dbo.Empleados emp ON emp.IDEmpleado = h.UsuarioCambioId AND emp.Estatus = 1
    WHERE h.SolicitudId = @solicitudId
    ORDER BY h.FechaCambio ASC;
END
GO
