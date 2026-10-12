using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Reportes
{
    public class mdl_Reporte_Visitas_ProgramadasCRM_VIEW
    {
        public IEnumerable<mdl_Reporte_Visitas_ProgramadasCRM> listado_visitas { get; set; }
        public IEnumerable<mdl_Reporte_Visitas_ProgramadasCRM_Grafica> info_grafica { get; set; }
        public mdl_Permisos_CRM permisos { get; set; }

    }
}
