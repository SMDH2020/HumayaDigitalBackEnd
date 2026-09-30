using HD_Finanzas.Modelos.Margenes;

namespace HD_Finanzas.Modelos.Margenes
{
    public class mdl_Margenes_Brutos_View
    {
        public IEnumerable<mdl_Margenes_Brutos> margenes { get; set; }
        public IEnumerable<mdl_Margenes_Brutos_Guias> guias { get; set; }
    }
}
