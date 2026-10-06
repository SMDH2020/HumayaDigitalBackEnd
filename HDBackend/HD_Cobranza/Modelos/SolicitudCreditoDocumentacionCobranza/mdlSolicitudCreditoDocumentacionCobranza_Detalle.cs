namespace HD_Cobranza.Modelos.SolicitudCreditoDocumentacionCobranza
{
    // Checklist de documentos requeridos de un folio; faltantes primero.
    public class mdlSolicitudCreditoDocumentacionCobranza_Detalle
    {
        public string? Folio { get; set; }
        public int IdDocumento { get; set; }
        public string? Documento { get; set; }
        public bool Cargado { get; set; }
    }
}
