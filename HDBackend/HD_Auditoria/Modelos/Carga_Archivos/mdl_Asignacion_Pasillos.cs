namespace HD_Auditoria.Modelos.Carga_Archivos
{
    public class mdl_Asignacion_Pasillos
    {
        public string folio {  get; set; }
        public List<mdl_TVPAsignacion_Pasillos> asignacion_pasillos {  get; set; }
        public int update_user {  get; set; }
    }
}
