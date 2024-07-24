using HD.Clientes.Modelos.Pedido_Impresion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.PrestamoClientes
{
    public class mdlPrestamo_Clientes_ObtenerDetalle
    {
        public mdlPrestamo_Cliente_Info? info { get; set; }
        public List<mdlPedido_Detalle_Financiamiento>? detallefinanciamiento { get; set; }
    }
}
