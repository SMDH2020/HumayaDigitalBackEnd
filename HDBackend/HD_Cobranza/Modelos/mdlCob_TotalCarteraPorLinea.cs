namespace HD_Cobranza.Modelos
{
    public class mdlCob_TotalCarteraPorLinea
    {
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public string linea { get; set; }
        public double mas90 { get; set; }
        public double mas60 { get; set; }
        public double mas30 { get; set; }
        public double mas15 { get; set; }
        public double de1a15 { get; set; }
        public double vencido { get; set; }
        public double porvencer { get; set; }
        public double activo { get; set; }
        public double totalcartera { get; set; }
        public double saldoafavor { get; set; }
        public double total { get; set; }
        public double juridico { get; set; }
    }
}
