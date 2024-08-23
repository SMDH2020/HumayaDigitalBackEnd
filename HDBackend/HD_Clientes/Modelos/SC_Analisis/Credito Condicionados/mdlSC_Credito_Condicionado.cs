using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados
{
    public class mdlSC_Credito_Condicionado
    {
        public mdl_fecha_compromiso? datos_fecha { get; set; }
        public mdldatos_notificacion? mdldatos { get; set; }
        public IEnumerable<mdlSolicitudCredito_Enviar>? mdlSolicitud { get; set; }
    }
}
