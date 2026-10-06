using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_CentroMonitoreo.Modelos.Dashboard
{
    public class mdl_Dashboard_Flota_Conectada_API
    {
        public int jd_org_id { get; set; }
        public string? organizacion { get; set; }
        public string? sucursal { get; set; }
        public bool es_flota_propia { get; set; }
        public int total_equipos { get; set; }
        public int equipos_conectados { get; set; }
        public int equipos_completos { get; set; }
        public mdl_Dashboard_DatosReportados datos_reportados { get; set; } = new mdl_Dashboard_DatosReportados();
    }
}
