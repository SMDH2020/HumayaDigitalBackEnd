namespace HD_Auditoria.Modelos.Carga_Archivos
{
    public class mdl_Cargar_Inventario_Fisico
    {
        public string folio {  get; set; }
        public int id_usuario { get; set; }
        public List<mdl_TVPInventario> inventario{ get; set; }
    }
}
