namespace HD.Clientes.Modelos.Facturar_Equipo
{
    public class mdlPEdidoFinanciamiento
    {
        public int docto { get; set; }
        public string? vencimiento { get; set; }
        public double importefinanciar { get; set; }
        public int dias { get; set; }
        public double tasa { get; set; }
        public double interes { get; set; }
        public double totalpagar { get; set; }
        public string? documento { get; set; }
    }
}
