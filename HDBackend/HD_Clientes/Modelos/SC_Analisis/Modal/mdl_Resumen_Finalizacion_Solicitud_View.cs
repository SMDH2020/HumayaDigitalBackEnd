using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis.Modal
{
    public class mdl_Resumen_Finalizacion_Solicitud_View
    {
        public mdlSCAnalisis_Decicion? estado { get; set; }
        public IEnumerable<mdl_Tabla_Diferencias_Tasas>? resumen_tasas { get; set; }
        public IEnumerable<mdl_Tabla_Diferencias_Facturacion>? resumen_facturacion { get; set; }
    }
}
