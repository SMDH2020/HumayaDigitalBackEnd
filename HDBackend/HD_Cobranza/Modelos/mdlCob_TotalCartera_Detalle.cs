namespace HD_Cobranza.Modelos
{
    public class mdlCob_TotalCartera_Detalle
    {
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public string? departamento { get; set; }
        public int idcliente { get; set; }
        public string? razonsocial { get; set; }
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
        //public string? id { get; set; }
        //public string? estatus { get; set; }
        //public string? linea { get; set; }
        //public int idsucursal { get; set; }
        //public string? sucursal { get; set; }
        //public string? idcliente { get; set; }
        //public string? razonsocial { get; set; }
        //public string? documento { get; set; }
        //public string? vencimiento { get; set; }
        //public int diasvencido { get; set; }
        //public double interesdiariobase { get; set; }
        //public double saldo { get; set; }
        //public double interesbase { get; set; }
        //public double importe { get; set; }
    }
}

