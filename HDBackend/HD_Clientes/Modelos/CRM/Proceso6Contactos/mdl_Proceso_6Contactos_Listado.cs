using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Proceso6Contactos
{
    public class mdl_Proceso_6Contactos_Listado
    {
        public int id_c6c { get; set; }
        public int id_CEE { get; set; }
        public string folio { get; set; }
        public int orden { get; set; }
        public int idcliente { get; set; }
        public string razon_social { get; set; }
        public int idlinea { get; set; }
        public string linea { get; set; }
        public string idmodelo { get; set; }
        public string modelo { get; set; }
        public string fase { get; set; }
        public string fecha_entrega { get; set; }
        public string fecha_contacto { get; set; }
        public bool? realizado { get; set; }
        public string comentarios { get; set; }
        public string telefono { get; set; }
    }
}
