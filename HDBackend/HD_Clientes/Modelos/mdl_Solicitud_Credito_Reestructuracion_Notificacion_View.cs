using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos
{
    public class mdl_Solicitud_Credito_Reestructuracion_Notificacion_View
    {
        public IEnumerable<mdlSolicitudCredito_Documentacion>? documentacion { get; set; }
        public mdl_Notificar? notificar { get; set; }

        public IEnumerable<mdlSolicitudCredito_Enviar>? mdlSolicitud { get; set; }
    }
}
