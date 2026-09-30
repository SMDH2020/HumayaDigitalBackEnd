using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Cotizaciones
{
    public class mdl_Catalogo_ServicioCRMID_View
    {
        public IEnumerable<mdl_Opciones_Lineas_Ventas> LineasVenta { get; set; }
        public mdl_Catalogo_Servicio_CRMID Servicio { get; set; }
    }
}
