using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos
{
    public class mdl_Obtener_Solicitudes_View
    {
        public IEnumerable<mdl_datos_Solicitud> Solicitudes { get; set; }
        
        public IEnumerable<string> Contacto_servicio { get; set; }
        public IEnumerable<string> Contacto_refacciones { get; set; }
        public IEnumerable<string> Contacto_ventas { get; set; }



    }
}
