using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Auditoria.Modelos.Reporteria
{
    public class mdl_Reporte_Primer_Conteo_View
    {
        public IEnumerable<mdl_Reporte_Primer_Conteo_Detalle> detalle { get; set; }
        public mdl_Reporte_Primer_Conteo_Resumen resumen { get; set; }
    }
}
