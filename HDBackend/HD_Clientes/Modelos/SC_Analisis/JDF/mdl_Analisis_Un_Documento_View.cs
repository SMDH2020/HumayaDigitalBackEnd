using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis.JDF
{
    public class mdl_Analisis_Un_Documento_View
    {
        public mdlJDFAnalisis_Decicion_un_documento? documento { get; set; }

        public IEnumerable<mdlSolicitudCredito_Enviar>? mdlSolicitud { get; set; }
    }
}
