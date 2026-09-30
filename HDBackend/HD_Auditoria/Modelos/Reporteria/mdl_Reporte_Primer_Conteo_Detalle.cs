namespace HD_Auditoria.Modelos.Reporteria
{
    public class mdl_Reporte_Primer_Conteo_Detalle
    {
        public string familia { get; set; }
        public string sku { get; set; }
        public string descripcion { get; set; }
        public string posicion { get; set; }
        public float existencia { get; set; }
        public float conteo { get; set; }
        public float precio_unitario { get; set; }
        public float diferencias { get; set; }
        public string tipo_diferencia { get; set; }
        public float importe_dif { get; set; }
        public string comentario { get; set; }
        public string ubicacion_correcta { get; set; }
        public string unidad_medida { get; set; }
        public float importe_existencia { get; set; }
        public float importe_cont_fisico { get; set; }
        public float porc_dif { get; set; }
        public float justificadas { get; set; }
        public float importe_justificadas { get; set; }
        public float no_justificadas { get; set; }
        public float importe_no_justificadas { get; set; }

    }
}
