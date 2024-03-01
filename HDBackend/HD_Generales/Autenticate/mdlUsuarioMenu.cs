using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Generales.Autenticate
{
    public class mdlUsuarioMenu
    {
        public int idrel { get; set; }
        public int idusuario { get; set; }
        public int idmenu { get; set; }
        public int idmodulo { get; set; }
        public bool estatus { get; set; } = true;
        public string? usuario { get; set; } = "";

    }
}
