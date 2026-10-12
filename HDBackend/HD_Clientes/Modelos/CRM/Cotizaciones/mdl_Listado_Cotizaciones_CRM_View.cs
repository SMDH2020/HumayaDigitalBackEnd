using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Cotizaciones
{
    public class mdl_Listado_Cotizaciones_CRM_View
    {
        public IEnumerable<mdl_Listado_Cotizaciones_CRM> cotizaciones { get; set; }
        public mdl_Permisos_CRM permisos { get; set; }
    }
}
