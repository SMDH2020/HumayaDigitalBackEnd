using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Proceso6Contactos
{
    public class mdl_Filtro_Responsable_C6C
    {
        public int id_resposanble { get; set; }   // alias tal cual lo devuelve el SP
        public string responsable { get; set; }
        public string area { get; set; }
        public int id_sucursal { get; set; }
        public int idestado { get; set; }
    }
}
