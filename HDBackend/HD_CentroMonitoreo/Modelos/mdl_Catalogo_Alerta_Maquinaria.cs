using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_CentroMonitoreo.Modelos
{
    public class mdl_Catalogo_Alerta_Maquinaria
    {
        public int id_alerta { get; set; }
        public string descripcion_alerta { get; set; }
        public string descripcion_cliente { get; set; }
        public int createuser { get; set; }
        public DateTime createdate { get; set; }
        public int? updateuser { get; set; }
        public DateTime? updatedate { get; set; }
    }
}
