namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    /// <summary>Rango de periodos inclusivo (ejercicio + mes en ambos extremos).</summary>
    public class mdl_Dashboard_Periodo
    {
        public int ejercicio_inicio { get; set; }
        public int periodo_inicio { get; set; }
        public int ejercicio_fin { get; set; }
        public int periodo_fin { get; set; }
    }
}
