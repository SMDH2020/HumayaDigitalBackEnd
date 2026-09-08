namespace HD_Ventas.Modelos
{
    public class mdl_Detalle_Cotizacion
    {
        public string folio {  get; set; }
        public string modelo { get; set; }
        public string mdl_descripcion {  get; set; }
        public string caracteristicas { get; set; }
        public float precio_lista { get; set; }
        public float descuento { get; set; }
        public float precio_final {  get; set; }
        public string moneda {  get; set; }
    }
}
