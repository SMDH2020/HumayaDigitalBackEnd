using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Listados_InvoiceMovimientos_View
    {
        public IEnumerable<mdl_Listado_Invoice> Invoice { get; set; }
        public IEnumerable<mdl_Listado_MovimientoContable> Movimientos { get; set; }
    }
}
