namespace HD.Clientes.Modelos
{
    public class mdlClientesDomicilioList
    {
        public int idcliente { get; set; }

        public int orden { get; set; }

        public int idestado { get; set; }

        public string? estado { get; set; } = "";

        public int idmunicipio { get; set; }

        public string? municipio { get; set; } = "";

        public int idlocalidad { get; set; }

        public string? localidad { get; set; } = "";
        public string? codigo_postal { get; set; } = "";

        public string? direccion { get; set; } = "";

        public string? tipodomicilio { get; set; } = "";
        public string? idtipodomicilio { get; set; }

        public bool principal { get; set; }

        public string? referencia1 { get; set; } = "";

        public string? referencia2 { get; set; } = "";

        public bool estatus { get; set; } = true;

        public string? usuario { get; set; } = "";
    }
}
