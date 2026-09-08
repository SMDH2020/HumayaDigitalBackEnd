using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.Eventos
{
    public class mdl_evento_notificacion
    {
        public int idlog { get; set; }
        public string? titulo { get; set; }
        public string? subtitulo { get; set; }
        public string? mensaje { get; set; }
        public string? portafolio { get; set; }
        public string? parametro { get; set; }
        public string? estado { get; set; }
        public string? cliente { get; set; }
        public string? referencia { get; set; }
        public string? redireccion { get; set; }
        public string? redireccionweb { get; set; }

    }
}
