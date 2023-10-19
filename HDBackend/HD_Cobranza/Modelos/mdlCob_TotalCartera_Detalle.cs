namespace HD_Cobranza.Modelos
{
    public class mdlCob_TotalCartera_Detalle
    {
        public string? id { get; set; }
        public string? estatus { get; set; }
        public string? linea { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public string? idcliente { get; set; }
        public string? razonsocial { get; set; }
        public string? documento { get; set; }
        public string? vencimiento { get; set; }
        public int diasvencido { get; set; }
        public double interesdiariobase { get; set; }
        public double saldo { get; set; }
        public double interesbase { get; set; }
        public double importe { get; set; }
    }
}

