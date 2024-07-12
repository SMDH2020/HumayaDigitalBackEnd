namespace HD_Cobranza.Modelos.RecuperacionCartera
{
    public class mdlRecuperacionObjetivo
    {
        public int periodo { get; set; }
        public string mes { get; set; }
        public double total_cartera { get; set; }
        public double cartera_activa { get; set; }
        public double cartera_porvencer { get; set; }
        public double cartera_vencida { get; set; }
        public double recuperacion_cartera_activa { get; set; }
        public double recuperacion_cartera_porvencer { get; set; }
        public double recuperacion_cartera_vencida { get; set; }
        public double objetivo { get; set; }
        public double recuperado { get; set; }
        public double porc { get; set; }
    }
}
