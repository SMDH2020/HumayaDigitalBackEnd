using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Finanzas.Modelos.RotacionInventario
{
    public class mdl_RotacionCXC_View
    {
        public bool editor_guia { get; set; }
        public IEnumerable<mdl_RotacionCXC>? rotacion { get; set; }

    }
}
