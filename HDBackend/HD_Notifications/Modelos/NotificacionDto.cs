using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Notifications.Modelos
{
    public class NotificacionDto
    {
        public int idencabezado { get; set; }

        public string? Titulo { get; set; }
        public string? Mensaje { get; set; }
        public string? redireccion { get; set; }
        public int evento { get; set; }
        public DateTime fecha_evento { get; set; }
        public string? usuario { get; set; }
        //public IEnumerable<string>? idSuscripcion { get; set; }
        public string? usuarioNotificar { get; set; }
        public string? parametro { get; set; }
    }
}
