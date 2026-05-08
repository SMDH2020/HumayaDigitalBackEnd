namespace HD_Auditoria.Modelos.Carga_Archivos
{
    public class mdl_Cargar_Inventario_Ajustes
    {
        public string folio {  get; set; }
        public string tipo_ajuste { get; set; }
        public int id_usuario { get; set; }
        public List<mdl_TVPAjustes> ajustes { get; set; }
    }
}
