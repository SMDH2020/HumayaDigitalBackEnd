using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Reportes
{
    public class mdl_Reporte_Cobertura_VisitasCRM_View
    {
        public IEnumerable<mdl_Reporte_Cobertura_Visitas_CRM> listado { get; set; }
        public mdl_Permisos_CRM permisos { get; set; }
    }
}
