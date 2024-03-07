using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Generales
{
    public class mdlRelUsuarioMenu
    {
        public int idrel { get; set; }
        public int idmenu { get; set; }
        public string? descripcion { get; set; } = "";
        public bool acceso { get; set; }
    }
}
