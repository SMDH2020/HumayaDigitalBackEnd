namespace HD_Auditoria.Modelos.Reporteria
{
    public class mdl_Reporte_Simplificado_Detalle
    {
        public string familia { get; set; }
        public string sku { get; set; }
        public string descripcion { get; set; }
        public string posicion { get; set; }
        public float existencia { get; set; }
        public float conteo { get; set; }
        public float diferencias { get; set; }
        public string tipo_diferencia { get; set; }
        public float importe_dif { get; set; }
        public string comentario { get; set; }
    }
}
