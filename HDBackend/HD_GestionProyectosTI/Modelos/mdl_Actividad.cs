namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_Actividad
    {
        public int idactividad { get; set; }
        public int idsolicitud { get; set; }
        public string? folio { get; set; }                 // solo en ListadoPorDeveloper
        public string? titulo_solicitud { get; set; }       // solo en ListadoPorDeveloper

        public string descripcion { get; set; } = "";
        public decimal estimacion_horas { get; set; }
        public int idusuario_developer { get; set; }
        public string estado { get; set; } = "Pendiente";   // Pendiente | En progreso | Terminada

        public DateTime? fecha_inicio_real { get; set; }
        public DateTime? fecha_fin_real { get; set; }
        public DateTime fecha_creacion { get; set; }
    }
}
