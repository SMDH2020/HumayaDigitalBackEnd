using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.CondonacionIntereses
{
    public class mdl_Guarda_Usuarios_Autorizan_Condonacion
    {
        public int? id { get; set; }
        public int usuario_autoriza { get; set; }
        public double porcentaje_normal { get; set; }
        public double porcentaje_moratorio { get; set; }
        public bool Notificar { get; set; }
        public int usuario { get; set; }
    }
}
