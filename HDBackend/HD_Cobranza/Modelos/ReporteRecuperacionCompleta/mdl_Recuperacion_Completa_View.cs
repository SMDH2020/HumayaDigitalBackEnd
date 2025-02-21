using HD_Cobranza.Modelos.ReporteRecuperacionCompleta;

namespace HD_Cobranza.Modelos.ReporteRecuperacionCompleta
{
    public class mdl_Recuperacion_Completa_View
    {
        public IEnumerable<mdl_Recuperacion_Completa> total { get; set; }
        public IEnumerable<mdl_Recuperacion_Completa> operacion { get; set; }
        public IEnumerable<mdl_Recuperacion_Completa> revolvente { get; set; }
        public IEnumerable<mdl_Recuperacion_Completa> especial { get; set; }
        public IEnumerable<mdl_Recuperacion_Completa> juridico { get; set; }
    }
}
