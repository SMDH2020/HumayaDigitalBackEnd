namespace HD.Clientes.Modelos
{
    public class mdlClientesDocumentacion_Guardar
    {
        public int idclientedocumento { get; set; }
        public int idcliente { get; set; }
        public int iddocumento { get; set; }
        public int orden { get; set; }
        public string? documento { get; set; } = "";
        public string? extension { get; set; } = "";
        public string? vigencia { get; set; } = "";
        public string? comentarios { get; set; } = "";
        public bool estatus{ get; set; }
        public string? usuario { get; set; } = "";
    }
}
