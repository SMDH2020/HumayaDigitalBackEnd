namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    /// <summary>Fila del primer select del SP: KPIs del rango consultado y del periodo anterior.</summary>
    public class mdl_Dashboard_Kpis_Fila
    {
        public int organizaciones { get; set; }
        public int organizaciones_anterior { get; set; }
        public int equipos_registrados { get; set; }
        public int equipos_registrados_anterior { get; set; }
        public int equipos_reportando { get; set; }
        public int equipos_reportando_anterior { get; set; }
        public int mensajes_leidos { get; set; }
        public int mensajes_leidos_anterior { get; set; }
    }
}
