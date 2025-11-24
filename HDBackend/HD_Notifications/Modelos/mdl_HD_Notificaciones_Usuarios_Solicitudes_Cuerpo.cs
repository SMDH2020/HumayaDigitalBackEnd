using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.Modelos
{
    public class mdl_HD_Notificaciones_Usuarios_Solicitudes_Cuerpo
    {
        public int idlog { get; set; }
        public string? mensaje { get; set; }
        public int idmodulo { get; set; }
        public string? redireccion { get; set; }

        public string? parametro { get; set; }
        public string? cliente { get; set; }

    }
}
