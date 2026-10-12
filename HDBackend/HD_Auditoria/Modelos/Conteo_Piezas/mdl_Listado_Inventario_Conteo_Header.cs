namespace HD_Auditoria.Modelos.Conteo_Piezas
{
    public class mdl_Listado_Inventario_Conteo_Header
    {
        public string folio { get; set; }
        public int id_sucursal { get; set; }
        public string sucursal { get; set; }
        public string tipo_inventario { get; set; }
        public string periodo { get; set; }
        public string ult_actualizacion { get; set; }
        public string responsable_alm { get; set; }
        public string auditor_ppal { get; set; }
        public string estatus { get; set; }
        public float precision_inv { get; set; }
        public float confiabilidad_inv { get; set; }
        public float confiabilidad_mon { get; set; }
        public float confiabilidad_loc { get; set; }
        public bool habilitar { get; set; }
        public string fecha_limite_just { get; set; }

    }
}
