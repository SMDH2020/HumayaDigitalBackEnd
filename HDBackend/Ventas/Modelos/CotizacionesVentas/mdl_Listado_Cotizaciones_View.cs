using HD.Clientes.Modelos.SC_Analisis.Modal;
using HD.Clientes.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.Modelos.CotizacionesVentas
{
    public class mdl_Listado_Cotizaciones_View
    {
        public mdl_Listado_Cotizaciones_Roles? roles { get; set; }
        public IEnumerable<mdl_Listado_Cotizaciones_Nuevo>? cotizaciones { get; set; }
    }
}
