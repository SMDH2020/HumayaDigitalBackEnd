namespace HD.Clientes.Modelos.Pedido_Impresion
{
    public class mdl_Pedido_Financiamiento_View
    {
        public string? folio { get; set; }
        public string? docto { get; set; }
        public string? vencimiento { get; set; }
        public short dias { get; set; }
        public double importefinanciar { get; set; }
        public double tasa { get; set; }
        public double interes { get; set; }
        public double totalpagar { get; set; }
    }
}
