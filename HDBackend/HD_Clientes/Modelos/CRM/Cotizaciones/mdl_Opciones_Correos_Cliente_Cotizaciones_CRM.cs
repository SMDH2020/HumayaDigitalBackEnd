using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Cotizaciones
{
    public class mdl_Opciones_Correos_Cliente_Cotizaciones_CRM
    {
        public int idcliente { get; set; }
        public int orden { get; set; }
        public string tipo_contacto { get; set; }
        public string valor { get; set; }
    }
}
