namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_RolTI
    {
        public int idusuario { get; set; }
        public string rol { get; set; } = "Usuario";   // Usuario | Developer | Admin
        public DateTime? fechaAsignacion { get; set; }
        public int? asignadoPor { get; set; }
    }

    public class mdl_RolTI_Asignar
    {
        public int idusuario { get; set; }
        public string rol { get; set; } = "";          // Developer | Admin
    }
}
