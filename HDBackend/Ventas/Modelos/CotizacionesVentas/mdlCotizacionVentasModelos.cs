namespace Ventas.Modelos.CotizacionesVentas
{
    public class mdlCotizacionVentasModelos
    {
        public int idmodelo {  get; set; }
        public int idlinea { get; set; }
        public string? linea_venta {  get; set; }
        public string? modelo { get; set; }
        public string? desc_modelo { get; set; }
        public float precio_lista { get; set; }
        public string? moneda {  get; set; }
        public int idpromocion { get; set; }
        public float precio_promocion { get; set; }
    }
}
