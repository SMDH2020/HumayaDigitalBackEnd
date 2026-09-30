using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Conciliacion_Ingresos_Analitica_Roles_View
    {
        public IEnumerable<mdl_Conciliacion_Ingresos_Analitica> Analitica { get; set; }
        public IEnumerable<mdl_Conciliacion_Ingresos_Analitica_Botones> Botones { get; set; }
    }
}
