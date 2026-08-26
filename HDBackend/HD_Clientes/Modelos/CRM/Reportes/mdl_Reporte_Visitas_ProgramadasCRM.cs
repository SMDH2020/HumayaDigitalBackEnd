using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM.Reportes
{
    public class mdl_Reporte_Visitas_ProgramadasCRM
    {
        public int id_visita { get; set; }
        public int idcliente { get; set; }
        public string razon_social { get; set; }
        public int idvendedor { get; set; }
        public string vendedor { get; set; }
        public int idsucursal { get; set; }
        public string sucursal { get; set; }
        public string tipo_visita { get; set; }
        public string visita { get; set; }
        public string fecha_visita { get; set; }
        public string estatus { get; set; }
        public string estatus_texto { get; set; }
        public string createdate { get; set; }
        public int idlinea { get; set; }
        public string linea { get; set; }
        public string? comentario { get; set; }
    }
}
