namespace HD_Ventas.Modelos
{
    public class mdlMCP_Scorecard_Vendedor
    {
        public string? sucursal { get; set; }
        public string? vendedor { get; set; }
        public string? linea { get; set; }
        public int unidades_objetivo { get; set; }
        public int unidades_vendidas { get; set; }
        public float importe_real { get; set; }
    }
}
