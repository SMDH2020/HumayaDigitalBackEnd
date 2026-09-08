namespace HD_Ventas.Modelos
{
    public class mdl_Cotizacion_Imprimir
    {
        public string folio { get; set; }
        public int idasesorventa { get; set; }
        public string asesorventa { get; set; }
        public int idcliente { get; set; }
        public string razon_social { get; set; }
        public string direccion {  get; set; }
        public string asunto { get; set; }
        public int idsucursal { get; set; }
        public string sucursal {  get; set; }
        public string fase_cotizacion { get; set; }
        public string moneda { get; set; }
        public int imprimir_precio_lista { get; set; }
        public string? terminos {  get; set; }
        public string vigencia { get; set; }
        public string detalle { get; set; }
    }
}
