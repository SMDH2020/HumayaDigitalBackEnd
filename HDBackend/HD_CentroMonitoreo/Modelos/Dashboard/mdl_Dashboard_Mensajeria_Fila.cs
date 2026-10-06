namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    /// <summary>Fila del segundo select del SP: totales de mensajeria del rango.</summary>
    public class mdl_Dashboard_Mensajeria_Fila
    {
        public int generados { get; set; }
        public int enviados { get; set; }
        public int entregados { get; set; }
        public int leidos { get; set; }
        public int pendientes { get; set; }
        public int con_error { get; set; }
        public string? error_causa { get; set; }
        public int error_total { get; set; }
    }
}
