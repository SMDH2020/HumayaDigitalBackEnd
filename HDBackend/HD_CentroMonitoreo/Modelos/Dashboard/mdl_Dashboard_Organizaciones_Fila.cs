namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    /// <summary>Fila del cuarto select del SP: resumen de organizaciones y equipos.</summary>
    public class mdl_Dashboard_Organizaciones_Fila
    {
        public int total { get; set; }
        public int con_equipos_reportando { get; set; }
        public int sin_reporte { get; set; }
        public int total_equipos { get; set; }
        public int tractores { get; set; }
    }
}
