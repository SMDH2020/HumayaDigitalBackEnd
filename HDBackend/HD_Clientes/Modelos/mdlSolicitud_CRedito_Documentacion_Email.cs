using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_CRedito_Documentacion_Email
    {
        public IEnumerable<mdlSolicitudCredito_Documentacion>? documentacion { get; set; }
        public mdl_Notificar? notificar { get; set; }

        public IEnumerable<mdlSolicitudCredito_Enviar>? mdlSolicitud { get; set; }
    }
}
