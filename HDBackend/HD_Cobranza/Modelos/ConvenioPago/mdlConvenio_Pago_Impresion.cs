using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.ConvenioPago
{
    public class mdlConvenio_Pago_Impresion
    {
        public mdlConvenio_Pago? cliente { get; set; }
        public List<mdlFacturasSeleccionadas>? facturas { get; set; }

    }
}
