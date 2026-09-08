using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Guarda_Validacion_Cliente_CRM
    {
        public int idcliente { get; set; }
        public bool validacion { get; set; }
        public int? usuario { get; set; }
    }
}
