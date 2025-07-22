namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Listado_Clientes_Gestionar_Prueba_2
    {
        public int ejercicio {  get; set; }
        public int periodo { get; set; }
        public int idcliente { get; set; }
        public string? razon_social { get; set; }
        public string? vencimiento { get; set; }
        public float objetivo { get; set; }
        public float capital {  get; set; }
        public float interes_normal {  get; set; }
        public float interes_moratorio {  get; set; }
        public float saldo_total { get; set; }
        public float recuperado { get; set; }
        public float saldo { get; set; }
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
