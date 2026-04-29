namespace HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados
{
    public class mdl_Analisis_100_detalle
    {
        public string? folio { get; set; }
        public int iddocumento { get; set; }
        public string? documento { get; set; }
        public string? estatus { get; set; }
        public bool enviar_revision { get; set; }
        public string? fecha_compromiso { get; set; }
        public string? comentarios { get; set; }
        public bool habilitar { get; set; }
        public bool tiene_documentacion { get; set; }
    }
}
