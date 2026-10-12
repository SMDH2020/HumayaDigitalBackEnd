namespace HD.Fiscal.Modelos
{
    public class mdl_Reversas_UUID_Vigente
    {
        public int idregistro { get; set; }
        public int document_no { get; set; }
        public string cust_ord_no { get; set; }
        public string invo_date { get; set; }
        public int ro_number { get; set; }
        public string special_inst { get; set; }
        public string series_code { get; set; }
        public string fiscal_document_no { get; set; }
        public int batch { get; set; }
        public string UUID { get; set; }
        public string rfc { get; set; }
        public string tipoComprobante { get; set; }
        public string condicionPago { get; set; }
        public bool cancelado { get; set; }
        public string? fechaCancelacion { get; set; }
        public float total { get; set; }
        public int batch_cancelacion { get; set; }

    }
}
