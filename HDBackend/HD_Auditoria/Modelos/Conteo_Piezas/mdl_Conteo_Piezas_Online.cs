namespace HD_Auditoria.Modelos.Conteo_Piezas
{
    public class mdl_Conteo_Piezas_Online
    {
        public int id_inv_fisico { get; set; }
        public string folio { get; set; }
        public int id_auditor { get; set; }
        public float conteo_fisico { get; set; }
        public bool ubicacion_ok { get; set; }
        public string modo_captura { get; set; }
        public int? id_sesion_off { get; set; }
        public string? observacion { get; set; }

    }
}
