using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Cotizaciones
{
    public class mdl_Cotizaciones_CRM_Folio_View
    {
        public IEnumerable<mdl_Opciones_Clientes_Cotizacion_CRM> Clientes { get; set; }
        public IEnumerable<mdl_Opciones_Asesores_Cotizaciones_CRM> Asesores { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> Origenes { get; set; }
        public IEnumerable<mdl_Opciones_Generales_CRM> TiposPago { get; set; }
        public mdl_Cotizaciones_CRM_Folio Cotizacion { get; set; }
        public IEnumerable<mdl_Cotizaciones_CRM_Folio_Detalle> Detalle { get; set; }
        public mdl_Permisos_CRM permisos { get; set; }
    }
}
