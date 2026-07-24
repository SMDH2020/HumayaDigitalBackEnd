using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HD_CentroMonitoreo.Modelos
{
    public class mdl_MaquinaDetalle
    {
        public string? jd_machine_id { get; set; }
        public string? jd_org_id { get; set; }
        public string? nombre { get; set; }
        public string? modelo { get; set; }
        public string? numero_serie { get; set; }

        public decimal? horas { get; set; }
        public DateTime? fecha_lectura { get; set; }

        public decimal? latitud { get; set; }
        public decimal? longitud { get; set; }
        public DateTime? fecha_ubicacion { get; set; }

        public string? estado { get; set; }
        public string? nivel_senal { get; set; }
        public DateTime? fecha_reporte { get; set; }

        public int? total_alertas { get; set; }
        public string? ultima_severidad { get; set; }
        public DateTime? ultima_fecha_alerta { get; set; }
    }
}