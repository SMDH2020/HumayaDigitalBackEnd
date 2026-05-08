namespace HD_Auditoria.Modelos.Carga_Archivos
{
    public class mdl_TVPInventario
    {
        public string familia {  get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public float existencia_orig {  get; set; }
        public string unidad_medida { get; set; }
        public float costo_unitario { get; set; }
        public string pasillo { get; set; }
        public string posicion { get; set; }
    }
}
