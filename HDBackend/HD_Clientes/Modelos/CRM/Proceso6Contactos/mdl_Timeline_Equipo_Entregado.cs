using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Proceso6Contactos
{
    public class mdl_Timeline_Equipo_Entregado
    {
        public int id_C6C { get; set; }
        public int id_CEE { get; set; }
        public string area { get; set; }
        public string area_texto { get; set; }
        public int id_responsable { get; set; }
        public string responsable { get; set; }
        public string fecha_compromiso { get; set; }
        public string fecha_contacto { get; set; }
        public string estado { get; set; }
        public int paso { get; set; }
    }
}
