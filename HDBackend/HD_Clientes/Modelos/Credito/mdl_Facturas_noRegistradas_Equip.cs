namespace HD.Clientes.Modelos.Credito
{
    public class mdl_Facturas_noRegistradas_Equip
    {
        public int idCliente {  get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public string? razonsocial { get; set; }
        public string? documento { get; set; }
        public string? fecha { get; set; }
        public string? vencimiento { get; set; }
        public float importeFactura {  get; set; }
        public float sado {  get; set; }
    }
}
