namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Facturas_Estado_Cuenta
    {
        public string? documento {  get; set; }
        public int idsucursal {  get; set; }
        public string? sucursal {  get; set; }
        public int ADR { get; set; }
        public string? estado {  get; set; }
        public int idcliente_HD {  get; set; }
        public int idcliente {  get; set; }
        public string? direccion { get; set; }
        public string? razonsocial {  get; set; }
        public string? descripcion {  get; set; }
        public string? serie_fiscal { get; set; }
        public string? folio_fiscal { get; set; }
        public string? fecha { get; set; }
        public string? vencimiento { get; set; }
        public float importefactura {  get; set; }
        public float importepagado {  get; set; }
        public float saldo {  get; set; }
        public float interes_pactado {  get; set; }
        public float interes_moratorio { get; set; }
        public float saldo_total {  get; set; }
        public int diasvencido { get; set; }
        public int referencia { get; set; }
    }
}
