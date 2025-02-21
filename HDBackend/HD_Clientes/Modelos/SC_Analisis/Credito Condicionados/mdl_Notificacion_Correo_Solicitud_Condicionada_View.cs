using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados
{
    public class mdl_Notificacion_Correo_Solicitud_Condicionada_View
    {
        public mdldatos_notificacion? mdldatos { get; set; }
        public IEnumerable<mdlSolicitudCredito_Enviar>? mdlSolicitud { get; set; }
    }
}
