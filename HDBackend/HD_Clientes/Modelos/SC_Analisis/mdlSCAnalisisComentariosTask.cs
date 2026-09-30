namespace HD.Clientes.Modelos.SC_Analisis
{
    public class mdlSCAnalisisComentariosTask
    {
        public string? folio { get; set; }
        public int idproceso { get; set; }
        public int iddocumento { get; set; }
        public int consecutivo { get; set; }
        public string? comentarios { get; set; }
        public string? estatus { get; set; }
        public string? usuario { get; set; }
    }
}
