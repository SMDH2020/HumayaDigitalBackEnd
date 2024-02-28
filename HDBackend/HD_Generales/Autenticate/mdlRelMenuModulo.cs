using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Generales.Autenticate
{
    public class mdlRelMenuModulo
    {
        public int idmenu { get; set; }
        public int idmodulo { get; set; }
        public string descripcion { get; set; } = "";
        public string nomenclatura { get; set; } = "";
        public bool estatus { get; set; } = true;
        public string? usuario { get; set; } = "";
    }
}
