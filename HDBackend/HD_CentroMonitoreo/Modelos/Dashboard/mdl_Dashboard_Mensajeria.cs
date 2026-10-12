namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    public class mdl_Dashboard_Mensajeria
    {
        public int generados { get; set; }
        public int enviados { get; set; }
        public int entregados { get; set; }
        public int leidos { get; set; }
        public int pendientes { get; set; }
        public int con_error { get; set; }
        public mdl_Dashboard_ErrorPrincipal error_principal { get; set; } = new mdl_Dashboard_ErrorPrincipal();
        public List<mdl_Dashboard_MensajeriaTipo> por_tipo { get; set; } = new List<mdl_Dashboard_MensajeriaTipo>();
    }
}
