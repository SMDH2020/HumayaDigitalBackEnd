using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postventa.Modelos.CodigoVendedoresEquip
{
    public class mdl_Codigos_View
    {
        public IEnumerable<string>? codigo { get; set; }
        public IEnumerable<string?>? vendedores { get; set; }

    }
}
