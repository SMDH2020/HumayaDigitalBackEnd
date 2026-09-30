namespace HD_Cobranza.Modelos
{
    public class mdlListadoPC
    {
        public string? folio { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public int idcliente { get; set; }
        public string? razon_social { get; set; }
        public string? asesor_ventas { get; set; }
        public string? celular { get; set; }

        public string? documento { get; set; }
        public string? serie_fiscal { get; set; }
        public string? folio_fiscal { get; set; }
        public double importe { get; set; }
        public int docto { get; set; }
        public int dias_financiamiento { get; set; }
        public string? vencimiento { get; set; }
        public double importefinanciar { get; set; }
        public double tasa { get; set; }
        public double interes { get; set; }
        public double totalpagar { get; set; }
        public double saldo { get; set; }
    }
}
