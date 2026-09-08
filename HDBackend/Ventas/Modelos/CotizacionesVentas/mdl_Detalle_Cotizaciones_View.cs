using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.Modelos.CotizacionesVentas
{
    public class mdl_Detalle_Cotizaciones_View
    {
        public mdl_Listado_Cotizaciones_Roles? roles { get; set; }
        public mdl_Listado_Cotizaciones_Nuevo? infoCotizacion { get; set; }
        public IEnumerable<mdl_Listado_Cotizaciones_Nuevo>? detalleCotizacion { get; set; }
    }
}
