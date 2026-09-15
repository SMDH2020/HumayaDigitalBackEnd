using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.CondonacionIntereses
{
    public class mdl_Obtener_Listado_Usuarios_Condonacion
    {
        public int id { get; set; }
        public int idusuario { get; set; }
        public string usuario { get; set; }
        public float porcentaje_normal { get; set; }
        public float porcentaje_moratorio { get; set; }
        public bool notificar { get; set; }
    }
}
