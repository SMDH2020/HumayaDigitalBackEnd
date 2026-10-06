namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    /// <summary>Fila del quinto select del SP y elemento de organizaciones.sucursales.</summary>
    public class mdl_Dashboard_Sucursal
    {
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public int total_equipos { get; set; }
        public int equipos_reportando { get; set; }
    }
}
