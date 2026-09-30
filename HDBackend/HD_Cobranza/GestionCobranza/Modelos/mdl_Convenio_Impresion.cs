using HD_Cobranza.GestionCobranza.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Cobranza.GestionCobranza.Modelos
{
    public class mdl_Convenio_Impresion
    {
        public mdl_Convenio_Guardar? cliente { get; set; }
        public List<mdl_Facturas_Seleccionadas>? facturas { get; set; }
    }
}
