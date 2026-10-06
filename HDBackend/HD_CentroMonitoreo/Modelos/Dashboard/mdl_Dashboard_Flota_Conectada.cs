using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    public class mdl_Dashboard_Flota_Conectada
    {
        public int jd_org_id { get; set; }
        public string? organizacion { get; set; }
        public int idsucursal { get; set; }
        public string? sucursal { get; set; }
        public bool es_flota_propia { get; set; }
        public int total_equipos { get; set; }
        public int equipos_conectados { get; set; }
        public int equipos_completos { get; set; }
        public int horometro { get; set; }
        public int combustible { get; set; }
        public int ubicacion { get; set; }
        public int alertas { get; set; }
    }
}
