namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Listado_Clientes_Gestionar_Prueba
    {
        public int idcliente { get; set; }
        public string? razon_social { get; set; }
        public string? vencimiento { get; set; }
        public float total_factura { get; set; }
        public float saldo_vencido { get; set; }
        public float saldo_porvencer { get; set; }
        public float recuperado { get; set; }
        public string? fecha_recuperacion { get; set; }
        public string? fecha_contacto { get; set; }
        public string? fecha_compromiso { get; set; }
        public string? convenio { get; set; }
        public string? objecion { get; set; }
        public string? observaciones { get; set; }
        public string? responsable { get; set; }
        public string? nombre_responsable { get; set; }
        public int idSucursal { get; set; }
    }
}
