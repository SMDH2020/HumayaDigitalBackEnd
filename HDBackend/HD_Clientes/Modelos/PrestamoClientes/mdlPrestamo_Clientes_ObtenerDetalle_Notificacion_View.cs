using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.PrestamoClientes
{
    public class mdlPrestamo_Clientes_ObtenerDetalle_Notificacion_View
    {
        public mdlPrestamo_Cliente_Info? info { get; set; }
        public List<mdlPedido_Detalle_Financiamiento>? detallefinanciamiento { get; set; }
        public IEnumerable<mdlSolicitudCredito_Enviar>? mdlSolicitud { get; set; }
    }
}
