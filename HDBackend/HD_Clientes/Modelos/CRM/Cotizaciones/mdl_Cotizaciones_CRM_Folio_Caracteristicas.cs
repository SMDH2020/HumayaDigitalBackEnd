using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Cotizaciones
{
    public class mdl_Cotizaciones_CRM_Folio_Caracteristicas
    {
        public string folio { get; set; }
		public int orden_articulo { get; set; }
        public int orden_caracteristica { get; set; }
        public string caracteristica { get; set; }
    }
}
