using HD.Clientes.Modelos.SC_Analisis.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class mdlAnalisis_Mhusa_Resultado
    {
        public mdlSCAnalisis_Pedido_Estado? estado { get; set; }
        public IEnumerable<mdlSolicitudCredito_Enviar>? socket { get; set; }
    }
}
