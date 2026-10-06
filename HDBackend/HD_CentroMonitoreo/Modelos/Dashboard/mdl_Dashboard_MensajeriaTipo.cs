namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    /// <summary>Fila del tercer select del SP y elemento de mensajeria.por_tipo.</summary>
    public class mdl_Dashboard_MensajeriaTipo
    {
        public string? tipo { get; set; }
        public int total { get; set; }
        public int leidos { get; set; }
        public int con_error { get; set; }
    }
}
