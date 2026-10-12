using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.CondonacionIntereses
{
    public class mdl_Reglas_Condonacion
    {
        public int idregla { get; set; }
        public string? concepto { get; set; }
        public int dias_aplicacion { get; set; }
        public double porc_limite { get; set; }
        public bool aplica_normal { get; set; }
        public bool aplica_moratorio { get; set; }
        public string? tipo_dias { get; set; }


    }
}
