namespace HD_Cobranza.Modelos.SolicitudCreditoDocumentacionCobranza
{
    // Las propiedades se llaman igual que las columnas del result set del SP.
    public class mdlSolicitudCreditoDocumentacionCobranza
    {
        public string? Folio { get; set; }
        public string? Tipo_Solicitud { get; set; }
        public int IdSucursal { get; set; }
        public string? Sucursal { get; set; }
        public int IdCliente { get; set; }
        public string? Cliente { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public int TotalRequeridos { get; set; }
        public int TotalCargados { get; set; }
        public int TotalFaltantes { get; set; }

        // Sin requisitos | Sin documentos | Completa | Incompleta
        public string? EstatusDocumentacion { get; set; }
    }
}
