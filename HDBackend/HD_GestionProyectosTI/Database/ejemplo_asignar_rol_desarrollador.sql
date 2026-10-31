-- Ejemplo: dar de alta a un usuario con el rol TI "DESARROLLADOR".
-- UsuarioId = AppMH.dbo.Empleados.IDEmpleado (mismo supuesto que el resto
-- del modulo, ver 17_NombresRealesUsuarios.sql).

DECLARE @UsuarioId INT = 123;  -- <-- reemplazar por el IDEmpleado real

INSERT INTO dbo.TI_UsuarioRoles (UsuarioId, RolId, Activo)
SELECT @UsuarioId, r.RolId, 1
FROM dbo.TI_Roles r
WHERE r.Codigo = 'DESARROLLADOR'
  AND NOT EXISTS (
        SELECT 1 FROM dbo.TI_UsuarioRoles ur
        WHERE ur.UsuarioId = @UsuarioId AND ur.RolId = r.RolId
      );
