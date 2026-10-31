namespace HD.Clientes.Modelos.SC_Analisis.Modal
{
    public class mdlSCAnalisis_Decicion
    {
        public bool habilitar { get; set; }
        public string? comentarios { get; set; }
        public string? estado { get; set; }
        public string? tiempoestimado { get; set; }
        public string? tiempotranscurrido { get; set; }
        public string? icono { get; set; }
        public int idproceso { get; set; }
        public int documentos_totales { get; set; }
        public int diferencias_documentos_tasas { get; set; }
        public int diferencias_documentos_facturacion { get; set; }

    }
}
