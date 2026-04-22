using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Mensajeria.Modelos
{
    public class mdl_Obtener_Chat_Mensajes
    {
        public int idMensaje { get; set; }
        public string numeroTelefono { get; set; }
        public string mensaje { get; set; }
        public string mensajePlantilla { get; set; }
        public string estatus { get; set; }
        public int createuser { get; set; }
        public string origino { get; set; }
        public string empleadoEnviado { get; set; }
        public string? idresponsable { get; set; }
        public string? responsable { get; set; }
        public string? modulo { get; set; }
        public string? fecha { get; set; }
    }
}
