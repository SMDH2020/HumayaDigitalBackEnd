using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis
{
    public class mdlAnalisis_Email_View
    {
        public IEnumerable<mdlCorreo_Notificacion>? notificacion { get; set; }
        public mdlAnalisis_Email? detalle { get; set; }
    }
}
