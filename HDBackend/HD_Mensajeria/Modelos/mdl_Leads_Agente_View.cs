using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Mensajeria.Modelos
{
    public class mdl_Leads_Agente_View
    {
        public IEnumerable<mdl_Leads_Agente>? Leads { get; set; }
        public IEnumerable<mdl_Empleados_Leads>? Empleados { get; set; }
        public IEnumerable<mdl_Sucursales_Leads>? Sucursales { get; set; }
        public IEnumerable<mdl_Areas_Leads>? Areas { get; set; }

    }
}
