using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.Modelos
{
    public class mdl_Notificaciones_Opciones_DDL
    {
        public IEnumerable<mdl_Modulos_Redireccion>? redirecciones { get; set; }
        public IEnumerable<mdl_Departamentos_DDL>? departamentos { get; set; }
    }
}
