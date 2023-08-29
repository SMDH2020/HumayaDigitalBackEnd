namespace HD.Clientes.Modelos
{
    public class mdlClientes_Domicilio
    {
        public int idcliente { get; set; }

        public int orden { get; set; } 

        public int idlocalidad { get; set; }

        public string direccion { get; set; }

        public string tipodomicilio { get; set; }

        public bool principal { get; set; } 

        public string referencia1 { get; set; } 

        public string referencia2 { get; set; }

        public bool estatus { get; set; } = true;

        public string? usuario { get; set; } = "";
    }
}
