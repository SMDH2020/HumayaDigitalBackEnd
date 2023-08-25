using System;

namespace HD.Clientes.Modelos
{
    public class mdlClientes_Hectareas
    {
        public int idcliente { get; set; }

        public short registro { get; set; }

        public double hectareas_propias { get; set; }

        public double hectareas_rentadas { get; set; }

        public double hectareas_ejidal { get; set; }

        public double hectareas_sociedad { get; set; }

        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
