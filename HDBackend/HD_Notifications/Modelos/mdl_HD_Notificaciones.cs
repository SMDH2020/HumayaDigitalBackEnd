using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.Modelos
{
    public class mdl_HD_Notificaciones
    {
        public int idnotificacion { get; set; }
        public string? mensaje { get; set; }
        public DateTime fecha_evento { get; set; }
        public bool repite { get; set; }
        public int dias { get; set; }
        public string? usuario { get; set; }

    }
}
