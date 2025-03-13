namespace Usados.Consultas.Inventario
{
    public class mdl_Cotizacion_Usados_PDF
    {
        public string modelo_descripcion {  get; set; }
        public string Marca { get; set; }
        public int ejercicio { get; set; }
        public float HP {  get; set; }
        public float horas { get; set; }
        public string estatus { get; set; }
        public int idsucursal { get; set; }
        public string sucursal {  get; set; }
        public float precio_lista { get; set; }
        public string? promocion {  get; set; }
        public DateTime? vigencia { get; set; }  
        public string? imagen { get; set; }
        public string? extension { get; set; }
    }
}
