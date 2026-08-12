using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Dashboard_CRM_View
    {
        public mdl_Dashboard_CRM_Solicitudes solicitudes { get; set; }
        public mdl_Dashboard_CRM_Cotizaciones cotizaciones { get; set; }
        public IEnumerable<mdl_Dashboard_CRM_Credito> lineasCredito { get; set; }

    }
}
