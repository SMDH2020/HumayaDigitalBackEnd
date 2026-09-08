namespace HD_Cobranza.Modelos.NotasDescuento
{
    public class mdl_Notas_Descuento
    {
        public int id { get;set;}
        public int idsucursal { get;set;}
        public string? sucursal {  get;set;}
        public int idcliente { get;set;}
        public string? razonsocial { get;set;}
        public string? documento {  get;set;}
        public string? serie { get;set;}
        public string? folio { get;set;}
        public string? fecha { get;set;}
        public string? vencimiento { get;set;}
        public float importefactura { get;set;}
        public float importepagado { get;set;}
        public float saldo_pendiente { get;set;}
    }
}
