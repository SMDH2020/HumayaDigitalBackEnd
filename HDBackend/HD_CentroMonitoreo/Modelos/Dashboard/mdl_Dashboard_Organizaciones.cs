namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    public class mdl_Dashboard_Organizaciones
    {
        public int total { get; set; }
        public int con_equipos_reportando { get; set; }
        public int sin_reporte { get; set; }
        public int total_equipos { get; set; }
        public int tractores { get; set; }
        public mdl_Dashboard_DatosReportados datos_reportados { get; set; } = new mdl_Dashboard_DatosReportados();
        public List<mdl_Dashboard_Sucursal> sucursales { get; set; } = new List<mdl_Dashboard_Sucursal>();
    }
}
