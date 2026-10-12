using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_CentroMonitoreo.Modelos
{
    public class mdl_Mensajes
    {
        public string organizacion { get; set; }
        public string cliente { get; set; }
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public string fecha_envio { get; set; }
        public string contacto { get; set; }
        public string estatus { get; set; }
        public string senddate { get; set; }
        public string id_registro { get; set; }
        public int orden { get; set; }
    }
}
