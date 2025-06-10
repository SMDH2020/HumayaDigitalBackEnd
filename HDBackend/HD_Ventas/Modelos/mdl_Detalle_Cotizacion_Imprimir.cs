using HD_Ventas.Modelos;

namespace HD_Ventas.Modelos
{
    public class mdl_Detalle_Cotizacion_Imprimir
    {
        public int idmodelo { get; set; }
        public string modelo { get; set; }
        public string descripcion { get; set; }
        //public List<mdl_Caracteristica_Modelo_Cotizacion_Imprimir> caracteristicas_json { get; set; }
        public string? caracteristicas_json { get; set; }
        public double precio_lista { get; set; }
        public double descuento { get; set; }
        public double precio_total { get; set; }
        public string? descripcion_promocion {  get; set; }
        public double precio_promocion {  get; set; }
        public string moneda { get; set; }
    }
}
