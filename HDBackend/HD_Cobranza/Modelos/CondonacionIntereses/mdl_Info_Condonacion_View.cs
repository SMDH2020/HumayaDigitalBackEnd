using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.CondonacionIntereses
{
    public class mdl_Info_Condonacion_View
    {
        public mdl_Limite_Condonacion_Usuario limites_usuario { get; set; }

        public IEnumerable<mdl_Reglas_Condonacion>? reglas { get; set; }
    }
}
