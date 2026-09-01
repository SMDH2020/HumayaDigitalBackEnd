namespace HD.Clientes.Modelos.CRM.IndicadoresCotizaciones
{
    /// <summary>
    /// Renglon del reporte de objetivo vs cotizaciones reales, por asesor, semana y linea.
    /// La linea 12 (Visitas) queda excluida: tiene su propio reporte.
    /// cumplimiento_vp es nullable: llega NULL cuando el objetivo es 0 y se pinta como N/A.
    /// </summary>
    public class mdl_IndicadoresCotizaciones_ReporteCotizaciones
    {
        public int idsemana { get; set; }
        public string? semana { get; set; } = "";
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }
        public int idestado { get; set; }
        public string? estado { get; set; } = "";
        public int idsucursal { get; set; }
        public string? sucursal { get; set; } = "";
        public int idvendedor { get; set; }
        public string? vendedor { get; set; } = "";
        public int idlinea { get; set; }
        public string? linea { get; set; } = "";
        public decimal objetivo_mensual { get; set; }
        public decimal objetivo { get; set; }
        public int real { get; set; }
        public decimal? cumplimiento_vp { get; set; }
    }
}
