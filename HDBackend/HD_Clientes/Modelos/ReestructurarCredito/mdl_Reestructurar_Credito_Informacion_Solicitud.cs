using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.ReestructurarCredito
{
    public class mdl_Reestructurar_Credito_Informacion_Solicitud
    {
        public mdlPedido_Condiciones_Venta? condiciones { get; set; }
        public IEnumerable<mdlPedido_Detalle_Financiamiento> detalles_financiamiento { get; set; }
    }
}
