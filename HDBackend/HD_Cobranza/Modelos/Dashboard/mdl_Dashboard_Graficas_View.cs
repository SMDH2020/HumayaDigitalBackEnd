using HD_Cobranza.Modelos.Dashboard;
using HD_Cobranza.Modelos.NewFolder;

namespace HD_Cobranza.Modelos.Dashboard
{
    public class mdl_Dashboard_Graficas_View
    {
        public IEnumerable<mdl_Dashboard_Total> total { get; set; }
        public IEnumerable<mdl_Dashboard_Comportamiento> comportamiento { get; set; }
        public IEnumerable<mdl_Dashboard_Comportamiento_Responsable> comportamiento_responsable { get; set; }
    }
}
