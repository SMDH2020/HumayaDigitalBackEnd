namespace HD_Finanzas.Modelos.Margenes
{
    public class mdlMargenes
    {
        public string? concepto { get; set; }
        public string? idadr { get; set; }
        public string? adr { get; set; }
        public string? idsucursal { get; set; }
        public string? sucursal { get; set; }
        public string? iddepartamento { get; set; }
        public string? departamento { get; set; }
        public double venta { get; set; }
        public double costo { get; set; }
        public double utilidad { get; set; }
        public double cantidad { get; set; }
        public bool unidades { get; set; }
    }
}
