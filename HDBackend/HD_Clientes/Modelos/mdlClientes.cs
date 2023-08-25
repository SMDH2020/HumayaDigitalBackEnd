namespace HD.Clientes.Modelos
{
    public class mdlClientes
    {
        public int idcliente { get; set; }

        public string rfc { get; set; }

        public string razon_social { get; set; }

        public string tipo_persona { get; set; }

        public string medio_contacto { get; set; }

        public int tiempo_agricultor { get; set; }

        public string agrupacion { get; set; }

        public string regimen_fiscal { get; set; }

        public string tipo_venta { get; set; }

        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
