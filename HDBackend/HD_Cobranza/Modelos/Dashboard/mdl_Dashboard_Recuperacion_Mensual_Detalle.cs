namespace HD_Cobranza.Modelos.Dashboard
{
    public class mdl_Dashboard_Recuperacion_Mensual_Detalle
    {
        public int idadr { get; set; }
        public string? adr { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public int idcliente { get; set; }
        public string? razonsocial { get; set; }
        public double importe_factura {  get; set; }
        public double pagado { get; set; }
        public double saldo { get; set; }
    }
}
