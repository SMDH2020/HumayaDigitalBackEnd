using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.Modelos.CondonacionIntereses
{
    public class mdl_Condonacion_Por_Folio_View
    {
        public mdl_Condonacion_Por_Folio? condonacion { get; set; }
        public List<mdl_Factura_Condonacion_Detalle> facturas { get; set; } = new List<mdl_Factura_Condonacion_Detalle>();
    }
}
