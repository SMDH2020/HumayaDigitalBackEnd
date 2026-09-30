using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD.Clientes.Modelos.CRM
{
    public class mdl_Dashboard_CRM_Solicitudes
    {
        public int creadas { get; set; }
        public int enProceso { get; set; }
        public int finalizadas { get; set; }
        public string? UltimaFechaFacturacion { get; set; }

    }
}
