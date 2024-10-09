namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Obtener_Convenios_Cliente_Gestionar
    {
        public string? tipo { get; set; }
        public int idfactura { get; set; }
        public string? documento { get; set; }
        public int idsucursal { get; set; }
        public int idcliente_HD { get; set; }
        public int idcliente { get; set; }
        public string? descripcion { get; set; }
        //public int id { get; set; }
        //public int idadr { get; set; }
        public string? serie_fiscal { get; set; }
        public string? folio_fiscal { get; set; }
        //public string? sucursal { get; set; }
        public string? fecha { get; set; }
        public string? vencimiento { get; set; }
        public double importefactura { get; set; }
        public double importepagado { get; set; }
        public double saldo { get; set; }
        public string? pagare { get; set; }
        public double interes_pactado { get; set; }
        public float interes_moratorio { get; set; }
        public double saldo_total { get; set; }
        public int diasvencido { get; set; }
        public float tasa {  get; set; }
        //public string? descripcion { get; set; }
        
        //public double intereses { get; set; }
        //public double interesbase { get; set; }
        //public double diasultimopago { get; set; }
        
        
    }
}
