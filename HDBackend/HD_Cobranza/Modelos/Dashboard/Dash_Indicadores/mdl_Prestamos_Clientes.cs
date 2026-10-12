using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.Dashboard.Dash_Indicadores
{
    public class mdl_Prestamos_Clientes
    {
        public int total_prestamos { get; set; }
        public int facturados_con_solicitud { get; set; }
        public int facturados_sin_solicitud { get; set; }
        public int solicitudes_sin_factura { get; set; }

    }
}
