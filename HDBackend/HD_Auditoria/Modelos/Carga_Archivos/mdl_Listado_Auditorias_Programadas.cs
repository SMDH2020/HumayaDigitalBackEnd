namespace HD_Auditoria.Modelos.Carga_Archivos
{
    public class mdl_Listado_Auditorias_Programadas
    {
        public int id_sucursal { get; set; }
        public string sucursal { get; set; }
        public string tipo_conteo { get; set; }
        public string fecha_programada { get; set; }
        public string fecha_ejecucion { get; set; }
        public string folio { get; set; }
        public int id_auditor_ppal { get; set; }
        public string auditor_principal { get; set; }
        public float diferencias { get; set; }
        public float porc_confiabilidad { get; set; }
        public float porc_avance { get; set; }
        public float conf_sku { get; set; }
        public string estatus { get; set; }
        public bool inv_cargado { get; set; }

    }
}
