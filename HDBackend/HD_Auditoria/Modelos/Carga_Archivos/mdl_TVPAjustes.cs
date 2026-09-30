namespace HD_Auditoria.Modelos.Carga_Archivos
{
    public class mdl_TVPAjustes
    {
        public string codigo {  get; set; }
        public string descripcion { get; set; }
        public float cantidad { get; set; }

        //SOLO TRANSITO
        public string? sucursal_origen { get; set; }
        public string? sucursal_dest { get; set; }
        public string? fecha_envio { get; set; }

        //SOLO SURTIDO
        public string? referencia_doc {  get; set; }
    }
}
