namespace HD.Clientes.Modelos
{
    public class mdlClientesDocumento_Listado
    {
        public int idclientedocumento { get; set; }
        public int idcliente { get; set; }
        public int iddocumento { get; set; }
        public int orden { get; set; }
        public string? documento { get; set; } = "";
        public string? vigencia { get; set; } = "";
        public int estado { get; set; }
        public string? comentarios { get; set; } = "";
    }
}
