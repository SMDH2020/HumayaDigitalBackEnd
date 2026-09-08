using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class mdlPedido_Detalle_Financiamiento_View
    {
        public mdlPedido_Detalle_Financiamiento_Info? info { get; set; }
        public IEnumerable<mdlPedido_Detalle_Financiamiento>? detalle_financiamiento { get; set; }
    }
}
