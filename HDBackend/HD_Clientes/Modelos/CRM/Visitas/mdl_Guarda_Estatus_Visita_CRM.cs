using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Visitas
{
    public class mdl_Guarda_Estatus_Visita_CRM
    {
        public int idvisita { get; set; }
        public string estatus { get; set; }
        public string comentario { get; set; }
        public int createuser { get; set; }
    }
}
