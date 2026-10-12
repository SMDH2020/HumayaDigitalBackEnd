using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Generales.Autenticate
{
    public class mdl_Rel_Usuario_Lineas_Venta
    {
        public int idrel { get; set; }
        public int idlinea { get; set; }
        public string? descripcion { get; set; } = "";
        public bool acceso { get; set; }
    }
}
