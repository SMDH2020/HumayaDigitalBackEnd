using HD_Cobranza.Modelos.Dashboard;
using HD_Cobranza.Modelos.NewFolder;

namespace HD_Cobranza.Modelos.Dashboard
{
    public class mdl_Dashboard_Graficas_Recuperacion_View
    {
        public IEnumerable<mdl_Dashboard_Objetivo> objetivo { get; set; }
        public IEnumerable<mdl_Dashboard_Recuperacion> recuperacion { get; set; }
        public IEnumerable<mdl_Dashboard_Recuperacion_Responsable> recuperacion_responsable { get; set; }
    }
}
