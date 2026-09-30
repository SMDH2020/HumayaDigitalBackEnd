namespace ProductoAliado.Modelos.Inventario
{
    public class mdl_Cotizacion_ProductoAliado
    {
        public string modelo_descripcion { get; set; }
        public string Marca { get; set; }
        public string modelo { get; set; }
        public float horas { get; set; }
        public string estatus { get; set; }
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public float precio_lista { get; set; }
        public string? promocion { get; set; }
        public DateTime? vigencia { get; set; }
        public string? imagen { get; set; }
        public string? extension { get; set; }
    }
}
