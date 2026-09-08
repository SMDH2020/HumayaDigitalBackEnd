namespace HD_Auditoria.Modelos.Reporteria
{
    public class mdl_Reporte_Primer_Conteo_Resumen
    {
        public float importe_total_inventario { get; set; }
        public float importe_faltante { get; set; }
        public float porc_faltante { get; set; }
        public float importe_sobrante { get; set; }
        public float porc_sobrante { get; set; }
        public float total_neto { get; set; }
        public float porc_total_neto { get; set; }
        public float confiabilidad { get; set; }
        public float confiabilidad_ubi { get; set; }
        public float conteo_faltante { get; set; }
        public float conteo_sobrante { get; set; }

    }
}
