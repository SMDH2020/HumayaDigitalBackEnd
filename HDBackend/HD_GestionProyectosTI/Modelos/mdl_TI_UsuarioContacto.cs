namespace HD_GestionProyectosTI.Modelos
{
    // Fila de salida de sp_TI_UsuarioRoles_ListadoPorRol: usuarios activos
    // con un rol activo dado, con nombre/correo ya resueltos contra
    // AppMH.dbo.Empleados.
    public class mdl_TI_UsuarioContacto
    {
        public int UsuarioId { get; set; }
        public string? Nombre { get; set; }
        public string? Correo { get; set; }
    }
}
