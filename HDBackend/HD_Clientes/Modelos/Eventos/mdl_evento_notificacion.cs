using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.Eventos
{
    public class mdl_evento_notificacion
    {
        public int idevento_usuario { get; set; }
        public string? titulo { get; set; }
        public string? subtitulo { get; set; }
        public string? comentario { get; set; }
    }
}
