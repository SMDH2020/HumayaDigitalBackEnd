using HD_Cobranza.Modelos.NewFolder;

namespace HD_Cobranza.Modelos.Dashboard
{
    public class mdl_Dashboard_Objetivo_Total_View
    {
        public IEnumerable<mdl_Dashboard_Objetivo_Total> objetivo_total { get; set; }
        public IEnumerable<mdl_Dashboard_Objetivo_Total> objetivo_cartera { get; set; }
        public IEnumerable<mdl_Dashboard_Objetivo_Total> objetivo_responsable { get; set; }
    }
}
