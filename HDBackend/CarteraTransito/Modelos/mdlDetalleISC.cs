using HD.Clientes.Modelos.SC_Analisis.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarteraTransito.Modelos
{
    public class mdlDetalleISC
    {
        public IEnumerable<mdl_Tabla_Diferencias_Facturacion>? resumen_facturacion { get; set; }
        public IEnumerable<mdl_Tabla_Documentos_Faltantes>? documentos_faltantes { get; set; }
    }
}
