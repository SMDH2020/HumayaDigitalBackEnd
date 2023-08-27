namespace HD.Clientes.Modelos
{
    public class mdlClientes_Datos_Contacto
    {
        public int idcliente { get; set; }

        public string tipo_contacto { get; set; }

        public string info { get; set; }

        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
