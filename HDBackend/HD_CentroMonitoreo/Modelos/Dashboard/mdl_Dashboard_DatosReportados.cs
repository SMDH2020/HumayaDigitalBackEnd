namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    /// <summary>Equipos que reportaron cada dato dentro del rango.</summary>
    public class mdl_Dashboard_DatosReportados
    {
        public int horometro { get; set; }
        public int ubicacion { get; set; }
        public int combustible { get; set; }
        public int alertas { get; set; }
        public int equipos_completos { get; set; }
        public int organizaciones_horometro { get; set; }
        public int organizaciones_ubicacionalertas { get; set; }
        public int organizaciones_combustible { get; set; }
        public int organizaciones_alertas { get; set; }
    }
}
