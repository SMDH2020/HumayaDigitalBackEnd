namespace HD_Ventas.Modelos
{
    public class mdl_Esquemas_por_Modelo
    {
        public int idmodelo { get; set; }
        public int idpromocion { get; set; }
        public string? modelo { get; set; }
        public string? descripcion_promocion { get; set; }
        public float precio_lista { get; set; }
        public float descuento { get; set; }
        public float precio_promocion { get; set; }
        public string? inicio_vigencia { get; set; }
        public string? vigencia { get; set; }

    }
}
