using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Visitas
{
    public class mdl_Agregar_Comentario_Visita
    {
        public int id_visita { get; set; }
        public string comentario { get; set; }
        public int usuario { get; set; }
    }
}
