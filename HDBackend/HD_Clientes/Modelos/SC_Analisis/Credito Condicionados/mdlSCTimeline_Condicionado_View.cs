using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados
{
    public class mdlSCTimeline_Condicionado_View
    {
        public mdlSCCredito_Responsables? responsables {  get; set; }
        public mdlSCTimeline_estado? estado { get; set; }
        public IEnumerable<mdlSCTimeline_detalle>? detalle { get; set; }
    }
}
