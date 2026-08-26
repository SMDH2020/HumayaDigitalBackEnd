using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Visitas
{
    public class mdl_Visita_TimeLine_CRM
    {
        public int id_visita { get; set; }
        public int orden { get; set; }
        public string comentario { get; set; }
        public int createuser { get; set; }
        public string usuario { get; set; }
        public DateTime createdate { get; set; }
    }
}
