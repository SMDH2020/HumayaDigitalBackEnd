using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.Dashboard.Dash_Indicadores
{
    public class mdl_Intereses_Condonados
    {
        public double total_interes_normal { get; set; }
        public double total_interes_moratorio { get; set; }
        public double total_intereses { get; set; }
        public double total_interes_normal_condonado { get; set; }
        public double total_interes_moratorio_condonado { get; set; }
        public double total_interes_condonado { get; set; }
    }
}
