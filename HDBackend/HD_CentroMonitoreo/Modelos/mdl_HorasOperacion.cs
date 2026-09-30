using System;
namespace HD_CentroMonitoreo.Modelos
{
    public class mdl_HorasOperacion
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public short? engine_state { get; set; }
        public string? detailed_state { get; set; }
    }
}
