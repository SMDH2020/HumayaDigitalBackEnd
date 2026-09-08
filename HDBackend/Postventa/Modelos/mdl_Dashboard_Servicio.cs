using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos
{
    public class mdl_Dashboard_Servicio
    {
        public string? Concepto { get; set; }
        public double total { get; set; }
        public double porcentaje { get; set; }
        public double margen { get; set; }
    }
}
