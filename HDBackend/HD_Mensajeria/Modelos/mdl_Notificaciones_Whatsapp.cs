using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Mensajeria.Modelos
{
    public class mdl_Notificaciones_Whatsapp
    {
        public int idMensaje { get; set; }
        public string? numeroTelefono { get; set; }
        public string? cliente { get; set; }
        public string? mensaje { get; set; }

    }
}
