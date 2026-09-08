using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis.JDF
{
    public class mdl_Facturacion_Evento_View
    {
        public mdlJDFAnalisis_Datos_Facturacion? documento { get; set; }

        public IEnumerable<mdlSolicitudCredito_Enviar>? mdlSolicitud { get; set; }
    }
}
