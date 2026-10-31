using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados
{
    public class mdl_Cargar_Documentacion_Aceptada_Condicionado_View
    {
        public mdl_Analisis_Documentacion_Aceptada_Condicionado_Completado? completado { get; set; }
        public mdldatos_notificacion? mdldatos { get; set; }
        public IEnumerable<mdlSolicitudCredito_Enviar>? mdlSolicitud { get; set; }
    }
}
