namespace HD_Ventas.Modelos
{
    public class mdlMCP_Scorecard_Vendedor
    {
        public string? sucursal { get; set; }
        public string? vendedor { get; set; }
        public string? linea { get; set; }
        public int? unidades_objetivo { get; set; }
        public double? importe_objetivo { get; set; }
        public int? unidades_vendidas { get; set; }
        public double? importe_real { get; set; }
        public double? costo_total { get; set; }
        public double? margen_total { get; set; }
    }
}
