using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.Modelos
{
    public class mdl_Notificacion_Usuarios_Solicitudes_View
    {
        public mdl_HD_Notificaciones_Usuarios_Solicitudes_Cuerpo notificacionCuerpo { get; set; }
        public IEnumerable<mdl_Usuarios_Especificos>? notificacionUsuarios { get; set; }
    }
}
