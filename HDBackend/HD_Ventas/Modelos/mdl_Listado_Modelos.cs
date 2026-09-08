namespace HD_Ventas.Modelos
{
    public class mdl_Listado_Modelos
    {
        public int idmodelo {  get; set; }
        public string modelo { get; set; }
        public string mdl_descripcion {  get; set; }
        public int idlinea {  get; set; }
        public string linea { get; set; }
        public string caracteristicas { get; set; }
        public int estatus { get; set; }
        public float costo_refacciones {  get; set; }
        public float costo_servicios { get; set; }
        public float precio_lista { get; set; }
        public string moneda { get; set; }
        public int categoria {  get; set; }
        public string categoria_descripcion { get; set; }
        public string tiene_fotografia {  get; set; }
    }
}
