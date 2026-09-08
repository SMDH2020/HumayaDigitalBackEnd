using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos.DashboardRefacciones
{
    public class mdl_Dashboard_Refacciones_View
    {
        public IEnumerable<mdl_Dashboard_Refacciones>? detalle { get; set; }
        public IEnumerable<mdl_Dashboard_Refacciones>? familia_10 { get; set; }
        public IEnumerable<mdl_Dashboard_Refacciones>? clientes_10 { get; set; }

        public IEnumerable<mdl_Dashboard_Refacciones>? pendiente1_10 { get; set; }

        public IEnumerable<mdl_Dashboard_Refacciones>? pendiente2_10 { get; set; }

    }
}
