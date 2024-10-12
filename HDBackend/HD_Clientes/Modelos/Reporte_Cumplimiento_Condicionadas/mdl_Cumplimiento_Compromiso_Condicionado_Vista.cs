using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.Reporte_Cumplimiento_Condicionadas
{
    public class mdl_Cumplimiento_Compromiso_Condicionado_Vista
    {
        public mdl_Cumplimiento_Compromiso_Condicionado resumen { get; set; }
        public IEnumerable<mdl_Cumplimiento_Compromiso_Condicionado_General> detalle { get; set; }
    }
}
