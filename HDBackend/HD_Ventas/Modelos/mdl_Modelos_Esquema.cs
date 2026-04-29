namespace HD_Ventas.Modelos
{
    public class mdl_Modelos_Esquema
    {
        public int idmodelo {  get; set; }
        public int idlinea { get; set; }
        public string? linea { get; set; }
        public string? modelo { get; set; }
        public string? descripcion { get; set; }
        public float precio_lista { get; set; }
        public int idpromocion { get; set; }
        public string? descripcion_promocion { get; set; }
        public float costo_refacciones { get; set; }
        public float costo_servicios { get; set; }
        public float precio_promocion { get; set; }
        public string eliminado { get; set; } = "N";
    }
}
