namespace Ventas.Modelos.CotizacionesVentas
{
    public class mdlCotizacionVentas
    {
        public string folio { get; set; } = "XXXXX";
        public string asesorventa { get; set; }
        public string cliente { get; set; }
        public string razon_social { get; set; }
        public string? vigencia { get; set; } = null;
        public string promocion { get; set; }
        public string moneda { get; set; } = "MXN";
        public bool imprimir_precio_lista { get; set; } = true;
        public string? terminos {  get; set; }
    }
}
