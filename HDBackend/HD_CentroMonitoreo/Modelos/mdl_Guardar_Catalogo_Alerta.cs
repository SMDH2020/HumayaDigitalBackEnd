using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_CentroMonitoreo.Modelos
{
    public class mdl_Guardar_Catalogo_Alerta
    {
        public int id_alerta { get; set; }   
        public string descripcion_alerta { get; set; }
        public string descripcion_cliente { get; set; }
        public int usuario { get; set; }
    }
}
