using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos.PartesFamilia
{
    public class mdl_Partes_Familia_Catalogo_View
    {
        public IEnumerable<mdl_Partes_Familia>? partes { get; set; }
        public IEnumerable<mdl_Familias?>? familias { get; set; }
        public IEnumerable<mdl_Familias?>? subfamilias1 { get; set; }
        public IEnumerable<mdl_Familias?>? subfamilias2 { get; set; }

    }
}
