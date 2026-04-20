using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Fiscal.Modelos
{
    public class mdl_Correccion_Incidencias_View
    {
        public IEnumerable<mdl_Listado_Invoice> Invoice { get; set; }
        public IEnumerable<mdl_Listado_Incidencia_VentasInternas> Ventas_Internas { get; set; }
        public IEnumerable<mdl_Listado_Incidencia_DescuentosNoTimbrados> Descuentos_Notimbrados { get; set; }
        public IEnumerable<mdl_Listado_Incidencia_Facturacion_NoRegistrada_EnVentas> Facturacion_NoRegistrada { get; set; }
        public IEnumerable<mdl_Listado_Incidencia_Facturacion_SinUuid> Facturación_SinUuid { get; set; }
        public IEnumerable<mdl_Reversas_UUID_Vigente> Reversas { get; set; }
        public mdl_Conciliacion_Ingresos_Analitica_Botones botones { get; set; }

    }
}
