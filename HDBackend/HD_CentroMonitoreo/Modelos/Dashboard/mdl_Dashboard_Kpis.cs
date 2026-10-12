namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    public class mdl_Dashboard_Kpis
    {
        public mdl_Dashboard_Valor organizaciones { get; set; } = new mdl_Dashboard_Valor();
        public mdl_Dashboard_Valor equipos_registrados { get; set; } = new mdl_Dashboard_Valor();
        public mdl_Dashboard_Reportando equipos_reportando { get; set; } = new mdl_Dashboard_Reportando();
        public mdl_Dashboard_Valor mensajes_leidos { get; set; } = new mdl_Dashboard_Valor();
    }
}
