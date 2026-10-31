using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Visitas
{
    public class mdl_Programar_Visita_CRM
    {
        public int? idvisita { get; set; }
        public int? idcliente { get; set; }
        public int? idvendedor { get; set; }
        public int tipo_visita { get; set; }
        public DateTime fecha_visita { get; set; }
        public string notas { get; set; }
        public int usuario { get; set; }
        public int linea { get; set; }
        public string estatus { get; set; }
    }
}
