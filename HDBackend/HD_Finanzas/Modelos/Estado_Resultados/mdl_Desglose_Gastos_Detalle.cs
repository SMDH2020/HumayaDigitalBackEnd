using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.Estado_Resultados
{
    public class mdl_Desglose_Gastos_Detalle
    {
        public string cuenta { get; set; }
        public string concepto { get; set; }
        public string tipo { get; set; }
        public string tipo_gasto { get; set; }
        public float importe { get; set; }

    }
}
