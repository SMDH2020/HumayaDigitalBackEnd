using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis.Modal
{
    public class mdlSC_Analisis_Credito
    {
        public IEnumerable<mdlSCAnalisis_Documentacion>? documentacion { get; set; }
        public mdlAnalisis_Email? email { get; set; }
    }
}
