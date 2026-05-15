namespace HD_Auditoria.Modelos.Conteo_Piezas
{
    public class mdl_Listado_Inventario_Conteo_Header_KPIs
    {
        public float total_inventario_sku { get; set; }
        public float registros_contados { get; set; }
        public float registros_diferencias { get; set; }
        public float registros_ubi_incorrecta { get; set; }
        public float total_inventario_dinero { get; set; }
        public float monto_total_diferencias { get; set; }
        public float conf_loc { get; set; }
        public float conf_inv { get; set; }
        public float conf_mon { get; set; }
        public float monto_total_inv { get; set; }
        public float monto_total_faltante { get; set; }
        public float porc_faltante { get; set; }
        public float monto_total_sobrante { get; set; }
        public float porc_sobrante { get; set; }
        public float total_neto { get; set; }
        public float avance { get; set; }

    }
}
