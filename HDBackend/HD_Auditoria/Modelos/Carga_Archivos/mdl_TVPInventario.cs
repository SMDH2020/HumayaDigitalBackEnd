namespace HD_Auditoria.Modelos.Carga_Archivos
{
    public class mdl_TVPInventario
    {
        public string franchise {  get; set; }
        public string part_no { get; set; }
        public string part_desc { get; set; }
        public float inmaster_oh_qty { get; set; }
        public string unidad_medida { get; set; } = "pieza";
        public float unit_cost { get; set; }
        public string pasillo { get; set; } = "por definir";
        public string bin_location { get; set; }
    }
}
