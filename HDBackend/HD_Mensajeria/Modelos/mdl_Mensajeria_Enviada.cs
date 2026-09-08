using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Mensajeria.Modelos
{
    public class mdl_Mensajeria_Enviada
    {
        public string? envio { get; set; }
        public int mensajes_enviados { get; set; }
        public int entregados { get; set; }
        public int leidos { get; set; }
        public int contestados { get; set; }
        public int errores { get; set; }

    }
}
