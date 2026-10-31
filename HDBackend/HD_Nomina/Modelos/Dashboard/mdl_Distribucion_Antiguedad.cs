using System;
using System.Collections.Generic;
using System.Text;

namespace HD_Nomina.Modelos.Dashboard
{
    public class mdl_Distribucion_Antiguedad
    {
        public int menos_1_año { get; set; }
        public int entre_1_3_años { get; set; }
        public int entre_3_5_años { get; set; }
        public int mas_5_años { get; set; }
        public decimal porc_menos_1_año { get; set; }
        public decimal porc_entre_1_3_años { get; set; }
        public decimal porc_entre_3_5_años { get; set; }
        public decimal porc_mas_5_años { get; set; }
    }
}
