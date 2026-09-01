using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Dashboard_CRM_Cotizaciones
    {
        public int creadas { get; set; }
        public int enProceso { get; set; }
        public int cerradas { get; set; }
        public int objetivo { get; set; }
        public string? UltimaFechaFacturacion { get; set; }
    }
}
