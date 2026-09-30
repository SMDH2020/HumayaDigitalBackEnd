using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Cotizaciones
{
    public class mdl_Cotizaciones_Servicio_CRM_Enviar_Correo
    {
        public string folio { get; set; }
        public string plantilla { get; set; }
        public List<string> destinatarios { get; set; }

        public string mensaje { get; set; }
    }
}
