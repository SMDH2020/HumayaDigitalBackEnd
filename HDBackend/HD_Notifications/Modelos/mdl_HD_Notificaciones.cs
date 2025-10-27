using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.Modelos
{
    public class mdl_HD_Notificaciones
    {
        public int idencabezado { get; set; }
        public string? mensaje { get; set; }
        public DateTime fecha_evento { get; set; }
        public int redireccion { get; set; }
        public string? tipo { get; set; }
        public string? dia { get; set; }

        public string? hora { get; set; }
        public int duracion { get; set; }

        public string? usuario { get; set; }

    }
}
