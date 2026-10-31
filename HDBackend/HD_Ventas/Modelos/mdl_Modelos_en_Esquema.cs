namespace HD_Ventas.Modelos
{
    public class mdl_Modelos_en_Esquema
    {
        public int idmodelo { get; set; }
        public int idlinea { get; set; }
        public string? linea { get; set; }
        public string? modelo { get; set; }
        public string? descripcion { get; set; }
        public float precio_lista { get; set; }
        public string? registrado_esquema { get; set; }
    }
}
