using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Generales.Autenticate
{
    public class mdlRelUsuarioImei
    {
        public int idusuario { get; set; }

        public string? imei { get; set; }

        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
