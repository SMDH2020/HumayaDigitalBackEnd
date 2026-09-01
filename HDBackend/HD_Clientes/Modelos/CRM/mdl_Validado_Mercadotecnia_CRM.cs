using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Validado_Mercadotecnia_CRM
    {
        public bool validado { get; set; }
        public int? id_ultimousuario { get; set; }
        public string? ultimousuario { get; set; }
        public DateTime? ultima_fecha { get; set; }
        public string? accion { get; set; }
    }
}
