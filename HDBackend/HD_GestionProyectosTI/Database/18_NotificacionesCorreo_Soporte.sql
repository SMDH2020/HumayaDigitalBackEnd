-- ============================================================================
-- HumayaDigital_GestionProyectosTI -- Procedimientos de SOLO LECTURA que dan
-- soporte a las notificaciones por correo del modulo (solicitud creada,
-- alcance aprobado, funcionalidad priorizada/enviada a revision, revision
-- resuelta). Script preparado para REVISION Y EJECUCION MANUAL. No se ha
-- ejecutado contra ningun servidor.
--
-- Estos 3 procedimientos NO modifican ningun dato -- solo resuelven a quien
-- hay que avisar y con que datos, para que el controller (en HD_Endpoints)
-- se los pase a HD_Notifications. El envio de correo en si NO vive aqui ni
-- en ningun AD de este proyecto (igual que ya aclara CLAUDE.md para
-- HD_Clientes/HD_Cobranza): el AD solo regresa datos, el envio lo hace el
-- controller con una clase de HD_Notifications.
--
-- Mismo supuesto de 17_NombresRealesUsuarios.sql: UsuarioId en las tablas
-- TI_* = AppMH.dbo.Empleados.IDEmpleado. Mismo servidor SQL, se cruza
-- directo con AppMH.dbo.Empleados sin necesitar otra cadena de conexion.
--
-- 1) sp_TI_UsuarioRoles_ListadoPorRol -- usuarios activos con un rol activo
--    dado (ej. 'JEFATURA_TI'), con su nombre/correo ya resueltos. Reemplaza
--    para este caso la necesidad de ir usuario por usuario.
-- 2) sp_TI_Funcionalidades_ContextoCorreo -- datos de una funcionalidad +
--    su solicitud + su solicitante, para los correos de "priorizada" y
--    "enviada a revision" (sp_TI_Funcionalidades_ProgramarInicial y
--    sp_TI_Funcionalidades_EnviarARevision no devuelven esto, son SP de
--    escritura que no se tocan).
-- 3) sp_TI_CambiosProgramacion_ContextoCorreo -- mismo dato pero a partir
--    de un CambioProgramacionId, para el correo de "revision resuelta"
--    (avisa a Jefatura de TI cuando el solicitante aprueba o rechaza).
-- ============================================================================

CREATE OR ALTER PROCEDURE dbo.sp_TI_UsuarioRoles_ListadoPorRol
    @RolCodigo NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ur.UsuarioId,
           CONCAT(e.Nombre, ' ', e.ApellidoPaterno, ' ', e.ApellidoMaterno) AS Nombre,
           e.Correo
    FROM dbo.TI_UsuarioRoles ur
    INNER JOIN dbo.TI_Roles r ON r.RolId = ur.RolId
    LEFT JOIN AppMH.dbo.Empleados e ON e.IDEmpleado = ur.UsuarioId AND e.Estatus = 1
    WHERE r.Codigo = @RolCodigo AND ur.Activo = 1 AND r.Activo = 1;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TI_Funcionalidades_ContextoCorreo
    @FuncionalidadId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT f.FuncionalidadId, f.Codigo, f.Nombre, f.MesProgramado, f.AnioProgramado,
           s.SolicitudId, s.Folio, s.Titulo, s.SolicitanteUsuarioId,
           CONCAT(e.Nombre, ' ', e.ApellidoPaterno, ' ', e.ApellidoMaterno) AS SolicitanteNombre,
           e.Correo AS SolicitanteCorreo
    FROM dbo.TI_Funcionalidades f
    INNER JOIN dbo.TI_VersionesAlcance v ON v.VersionAlcanceId = f.VersionAlcanceId
    INNER JOIN dbo.TI_Solicitudes s ON s.SolicitudId = v.SolicitudId
    LEFT JOIN AppMH.dbo.Empleados e ON e.IDEmpleado = s.SolicitanteUsuarioId AND e.Estatus = 1
    WHERE f.FuncionalidadId = @FuncionalidadId;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_TI_CambiosProgramacion_ContextoCorreo
    @CambioProgramacionId BIGINT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT c.CambioProgramacionId, c.FuncionalidadId, f.Codigo, f.Nombre,
           s.SolicitudId, s.Folio, s.Titulo, s.SolicitanteUsuarioId,
           CONCAT(e.Nombre, ' ', e.ApellidoPaterno, ' ', e.ApellidoMaterno) AS SolicitanteNombre,
           e.Correo AS SolicitanteCorreo,
           c.EstadoCambio, c.ComentarioResolucion
    FROM dbo.TI_CambiosProgramacion c
    INNER JOIN dbo.TI_Funcionalidades f ON f.FuncionalidadId = c.FuncionalidadId
    INNER JOIN dbo.TI_Solicitudes s ON s.SolicitudId = c.SolicitudId
    LEFT JOIN AppMH.dbo.Empleados e ON e.IDEmpleado = s.SolicitanteUsuarioId AND e.Estatus = 1
    WHERE c.CambioProgramacionId = @CambioProgramacionId;
END
GO
