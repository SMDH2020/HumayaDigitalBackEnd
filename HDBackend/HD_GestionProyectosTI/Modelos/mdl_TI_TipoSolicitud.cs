namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_TipoSolicitud
    {
        public int TipoSolicitudId { get; set; }
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
    }
}
