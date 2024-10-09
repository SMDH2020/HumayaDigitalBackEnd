namespace HD_Ventas.Modelos
{
    public class mdlListado_Facturacion_Maquinaria
    {
        public int idlinea { get; set; }
        public string? scorecard { get; set; }
        public int idSucursal { get; set; }
        public string? sucursal {  get; set; }
        public int idcliente { get; set; }
        public string? razonsocial { get; set; }
        public string? vendedor { get; set; }
        public string? fecha_venta {  get; set; }
        public string? primera_venta { get; set; }
        public string? serie {  get; set; }
        public int neconomico { get; set; }
        public string? modelo { get; set; }
        public float importe_venta { get; set; }
        public float utilidad { get; set; }
        public string? nip {  get; set; }
        public int cancelado { get; set; }
    }
}
