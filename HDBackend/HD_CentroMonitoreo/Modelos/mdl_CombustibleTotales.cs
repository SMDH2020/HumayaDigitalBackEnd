using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_CentroMonitoreo.Modelos
{
    public class mdl_CombustibleTotales
    {
        public decimal? total_historico { get; set; }
        public decimal? total_mes_actual { get; set; }
        public string? unidad_combustible { get; set; }
    }
}
