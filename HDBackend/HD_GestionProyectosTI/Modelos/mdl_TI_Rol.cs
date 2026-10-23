namespace HD_GestionProyectosTI.Modelos
{
    // Un rol activo del usuario autenticado dentro del modulo (independiente
    // de los roles/menus generales de Humaya Digital). Nombres de columna en
    // PascalCase porque asi fue creado y aprobado el esquema TI_* -- Dapper
    // mapea sin distinguir mayusculas/minusculas, pero se respeta la grafia
    // real de la base en vez de forzar snake_case.
    public class mdl_TI_Rol
    {
        public int RolId { get; set; }
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
    }
}
