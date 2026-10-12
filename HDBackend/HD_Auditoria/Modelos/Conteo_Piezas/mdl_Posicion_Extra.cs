namespace HD_Auditoria.Modelos.Conteo_Piezas
{
    public class mdl_Posicion_Extra
    {
        public int id_inv_fisico { get; set; }
        public string folio { get; set; }
        public string posicion_extra { get; set; }
        public float conteo_fisico { get; set; }
        public bool bloqueado { get; set; }
        public int id_auditor { get; set; } = 0;
    }
}
