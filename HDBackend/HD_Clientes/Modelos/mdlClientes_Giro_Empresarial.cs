namespace HD.Clientes.Modelos
{
    public class mdlClientes_Giro_Empresarial
    {
        public int idcliente_giro_empresarial { get; set; }

        public int idcliente { get; set; }

        public int idgiro_empresarial { get; set; }

        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
