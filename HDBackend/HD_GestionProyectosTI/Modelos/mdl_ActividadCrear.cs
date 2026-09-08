namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_ActividadCrear
    {
        public int idsolicitud { get; set; }
        public string descripcion { get; set; } = "";
        public decimal estimacion_horas { get; set; }
        public int idusuario_developer { get; set; }
    }
}
