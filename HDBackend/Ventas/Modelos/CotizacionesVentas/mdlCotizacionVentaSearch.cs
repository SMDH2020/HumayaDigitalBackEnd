namespace Ventas.Modelos.CotizacionesVentas
{
    public class mdlCotizacionVentaSearch
    {
        public mdlCotizacionVentas? cotizacion { get; set; }
        public List<mdlCotizacionVentaDetalle>? detalle{ get; set; }
        public mdlCotizacionVenta_rol? rol{ get; set; }
        public List<mdlCotizacionVentaDropdownlist>? clientes{ get; set; }
        public List<mdlCotizacionVentaDropdownlist>? asesorventas{ get; set; }
        public List<mdlCotizacionVentaDropdownlist>? esquemas{ get; set; }
        public List<mdlCotizacionVentasModelos>? modelos { get; set; }
    }
}
