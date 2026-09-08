using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Reportes
{
    public class mdl_Reporte_Cobertura_Visitas_CRM
    {
        public int idcliente { get; set; }
        public string razon_social { get; set; }
        public int idvendedor { get; set; }
        public string vendedor { get; set; }
        public int? idsucursal { get; set; }
        public string sucursal { get; set; }
        public int? idestado { get; set; }
        public string estado { get; set; }
        public int? idlocalidad { get; set; }
        public string localidad { get; set; }
        public string giros { get; set; }
        public int enero { get; set; }
        public int febrero { get; set; }
        public int marzo { get; set; }
        public int abril { get; set; }
        public int mayo { get; set; }
        public int junio { get; set; }
        public int julio { get; set; }
        public int agosto { get; set; }
        public int septiembre { get; set; }
        public int octubre { get; set; }
        public int noviembre { get; set; }
        public int diciembre { get; set; }
        public int total_visitas { get; set; }
    }
}
