namespace HD.Clientes.Modelos
{
    public class mdlClientes_Cultivo
    {
        public int idcliente { get; set; }

        public int registro { get; set; }

        public int idcultivo { get; set; }

        public string terreno{ get; set; } 

        public double hectareas { get; set; }

        public string seguro_cosecha  { get; set; }

        public string ciclo { get; set; } 

        public string tipo_riego { get; set; } 

        public string temporal { get; set; } 

        public double rendimiento { get; set; }

        public double precio { get; set; }

        public int mescosecha { get; set; }

        public bool estatus { get; set; } = true;

        public string? usuario { get; set; } = "";
    }
}

