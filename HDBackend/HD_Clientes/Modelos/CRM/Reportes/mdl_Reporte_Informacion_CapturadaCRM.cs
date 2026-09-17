using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Reportes
{
    public class mdl_Reporte_Informacion_CapturadaCRM
    {
        public int idcliente { get; set; }
        public string rfc { get; set; }
        public string razon_social { get; set; }
        public string tipo_persona { get; set; }
        public int? etiqueta { get; set; }
        public string etiqueta_texto { get; set; }
        public int? idvendedor { get; set; }
        public string vendedor { get; set; }
        public int? idsucursal { get; set; }
        public string sucursal { get; set; }
        public int validado { get; set; }
    }
}
