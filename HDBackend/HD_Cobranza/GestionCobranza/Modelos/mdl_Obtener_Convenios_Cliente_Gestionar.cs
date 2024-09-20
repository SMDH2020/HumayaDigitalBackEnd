namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Obtener_Convenios_Cliente_Gestionar
    {
        public int id { get; set; }
        public int idcliente { get; set; }
        public int idadr { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public string? documento { get; set; }
        public string? fecha { get; set; }
        public string? vencimiento { get; set; }
        public int diasvencido { get; set; }
        public string? descripcion { get; set; }
        public double importefactura { get; set; }
        public double importepagado { get; set; }
        public double saldo { get; set; }
        public double intereses { get; set; }
        public double interesbase { get; set; }
        public double diasultimopago { get; set; }
        public double interespactado { get; set; }
        public string? tipo {  get; set; }
        public double total => saldo + intereses;
    }
}
