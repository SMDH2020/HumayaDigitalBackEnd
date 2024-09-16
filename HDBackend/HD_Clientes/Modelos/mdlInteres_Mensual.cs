using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class mdlInteres_Mensual
    {
        public int idinteres { get; set; }

        public int ejercicio { get; set; }

        public int periodo { get; set; }
        public string? mes { get; set; }

        public double interes { get; set; }

        public string? documento { get; set; } = "";

        public string? extension { get; set; } = "";


        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
        public bool tienedocumento { get; set; } = false;
    }
}
