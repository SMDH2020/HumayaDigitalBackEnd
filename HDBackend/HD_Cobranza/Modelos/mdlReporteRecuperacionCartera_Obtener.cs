namespace HD_Cobranza.Modelos
{
    public class mdlReporteRecuperacionCartera_Obtener
    {
        public string? sucursal { get; set; }
        public int codigocliente { get; set; }
        public string? razonsocial { get; set; }
        public string? factura { get; set; }
        public float importe { get; set; }
        public float pago { get; set; }
        public DateTime? fecha { get; set; }
        public DateTime? fechapago { get; set; }
        public int dias { get; set; }
    }
}
