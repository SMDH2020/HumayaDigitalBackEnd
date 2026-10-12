using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Info_Cliente_Asesores_CRM
    {
        public int idvendedor { get; set; }
        public string asesor { get; set; }
        public int idlinea { get; set; }
        public string linea { get; set; }
        public string desde { get; set; }
        public int? id_ultimousuario { get; set; }
        public string? ultimousuario { get; set; }
        public DateTime? ultima_fecha { get; set; }
        public string? accion { get; set; }
    }
}
