using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Guarda_Clasificacion_Cliente_CRM
    {
        public int idcliente { get; set; }
        public string? lineas { get; set; } = "";
        public string? giros { get; set; } = ""; 
        public int superficie { get; set; }
        public int usuario { get; set; }
    }
}
