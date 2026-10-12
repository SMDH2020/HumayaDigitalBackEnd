namespace HD_Cobranza.Modelos.RecuperacionCartera
{
    public class mdlRecuperacionObjetivoView
    {
        public IEnumerable<mdlRecuperacionObjetivo> total { get; set; }
        public IEnumerable<mdlRecuperacionObjetivo> operacion { get; set; }
        public IEnumerable<mdlRecuperacionObjetivo> revolvente { get; set; }
    }
}
