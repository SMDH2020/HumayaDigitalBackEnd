using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.CondonacionIntereses
{
    public class mdl_Obtener_Usuario_Condonacion_ID
    {
        public int id { get; set; }
        public int usuario { get; set; }
        public float porcentaje_normal { get; set; }
        public float porcentaje_moratorio { get; set; }
        public bool notificar { get; set; }
    }
}
