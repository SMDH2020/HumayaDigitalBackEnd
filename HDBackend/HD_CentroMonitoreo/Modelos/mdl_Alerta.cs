
using System;

namespace HD_CentroMonitoreo.Modelos
{
    public class mdl_Alerta
    {
        public string? jd_alert_id { get; set; }
        public string? severidad { get; set; }
        public string? codigo_dtc { get; set; }
        public string? descripcion { get; set; }
        public DateTime? fecha_evento { get; set; }
    }
}