namespace Ventas.Modelos.CotizacionesVentas
{
    public class mdlCotizacionVentaDetalle
    {
        public string folio{ get; set; }
        public  int orden { get; set; }
        public string linea { get; set; }
        public string idlinea { get; set; }
        public string modelo { get; set; }
        public string idmodelo { get; set; }
        public string descripcion { get; set; }
        public double precio_lista { get; set; }
        public double descuento_promocion { get; set; }
        public double precio_promocion { get; set; }
        public double descuento_adicional { get; set; }
        public double precio_venta { get; set; }
    }
}
