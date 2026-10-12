using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis
{
    public class mdlSCTimeline_Condicionado_detalle
    {
        public int idproceso { get; set; }
        public string? proceso { get; set; }
        public string? tiempoestimado { get; set; }
        public string? creado { get; set; }
        public string? estado { get; set; }
        public string? control { get; set; }
        public string? habilitado { get; set; }
    }
}
