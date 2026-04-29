using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.Modelos
{
    public class mdl_Notificacion_Usuarios_Especificos_View
    {

        public mdl_HD_Notificaciones_Usuario_Especifico notificacionCuerpo { get; set; }
        public IEnumerable<mdl_Usuarios_Especificos>? notificacionUsuarios { get; set; }
    }
}
