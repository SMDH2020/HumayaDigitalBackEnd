using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.AgregarContacto
{
    public class mdlAgregarContacto
    {
        public int idmedio { get; set; }
        public int idcliente { get; set; }
        public string? tipomedio { get; set; }
        public string? mediocontacto { get; set; } = "";
        public string? medio { get; set; }
        public string? comentarios { get; set; } = "";
        public string? usuario { get; set; }
    }
}
