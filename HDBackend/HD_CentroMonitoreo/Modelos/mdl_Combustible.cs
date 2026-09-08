using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_CentroMonitoreo.Modelos
{
    public class mdl_Combustible
    {
        public decimal? combustible_pct { get; set; }
        public decimal? combustible_consumido { get; set; }
        public string? unidad_combustible { get; set; }
        public decimal? horas_motor { get; set; }
        public decimal? horas_idle { get; set; }
        public DateTime? fecha_lectura { get; set; }
    }
}
