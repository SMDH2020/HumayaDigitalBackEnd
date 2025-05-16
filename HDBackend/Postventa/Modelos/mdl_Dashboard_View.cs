using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos
{
    public class mdl_Dashboard_View
    {
        public string? dashboard_titulo { get; set; }
        public IEnumerable<mdl_Dashboard_Proyecciones>? proyecciones { get; set; }
        public IEnumerable<mdl_Dashboard_Servicio>? servicio { get; set; }
        public IEnumerable<mdl_Dashboard_Refacciones>? refacciones { get; set; }

        //public string? columnas { get; set; }
    }
}
