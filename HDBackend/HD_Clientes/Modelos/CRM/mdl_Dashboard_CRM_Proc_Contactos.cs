using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Dashboard_CRM_Proc_Contactos
    {
        public int contactos_mes { get; set; }
        public int pendientes { get; set; }
        public int realizados { get; set; }
        public float porcentaje_realizados { get; set; }

    }
}
