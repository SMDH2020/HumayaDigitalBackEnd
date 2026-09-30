using HD.Clientes.Modelos.SC_Analisis.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis.JDF
{
    public class mdlJDFAnalisis_Datos_Facturacion_Notificacion_View
    {
        public mdlJDFAnalisis_Datos_Facturacion? datos_facturacion { get; set; }
        public IEnumerable<mdlSolicitudCredito_Enviar>? mdlSolicitud { get; set; }
    }
}
