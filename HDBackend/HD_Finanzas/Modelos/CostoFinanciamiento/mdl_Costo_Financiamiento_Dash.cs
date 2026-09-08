using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.CostoFinanciamiento
{
    public class mdl_Costo_Financiamiento_Dash
    {
        public string grupo { get; set; }
        public string concepto { get; set; }
        public double real { get; set; }
        public double anterior { get; set; }
        public double anteriordiferencia { get; set; }
        public double porcentajeanterior {  get; set; }
    }
}
