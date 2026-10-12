namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    /// <summary>Respuesta completa de GET CentroMonitoreo/Dashboard.</summary>
    public class mdl_Dashboard_View
    {
        public mdl_Dashboard_Kpis kpis { get; set; } = new mdl_Dashboard_Kpis();
        public mdl_Dashboard_Mensajeria mensajeria { get; set; } = new mdl_Dashboard_Mensajeria();
        public mdl_Dashboard_Organizaciones organizaciones { get; set; } = new mdl_Dashboard_Organizaciones();
        public List<mdl_Dashboard_Atencion> atencion_requerida { get; set; } = new List<mdl_Dashboard_Atencion>();
        public List<mdl_Organizacion> organizaciones_mensajes_leidos { get; set; } = new List<mdl_Organizacion>();
        public List<mdl_Dashboard_Flota_Conectada_API> flota_conectada { get; set; } = new List<mdl_Dashboard_Flota_Conectada_API>();


        /// <summary>ISO 8601 sin zona horaria, ej. 2026-10-05T08:10:00</summary>
        public string fecha_actualizacion { get; set; } = string.Empty;
    }
}
