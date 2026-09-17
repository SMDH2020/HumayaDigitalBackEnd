namespace HD_Cobranza.Modelos.ReportePrestamosClientes
{
    public class mdlReportePrestamosClientes
    {
        public string? origen_registro { get; set; }
        public string? documento { get; set; }
        public string? serie { get; set; }
        public string? folio { get; set; }
        public string? razon_social { get; set; }
        public string? rfc { get; set; }
        public DateTime? vencimiento { get; set; }
        public decimal? importe_factura { get; set; }
        public decimal? importe_pagado { get; set; }
        public decimal? saldo { get; set; }
        public string? folio_solicitud { get; set; }
    }
}
