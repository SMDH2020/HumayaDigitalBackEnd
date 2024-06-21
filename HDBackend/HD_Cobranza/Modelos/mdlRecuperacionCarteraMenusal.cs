namespace HD_Cobranza.Modelos
{
    public class mdlRecuperacionCarteraMenusal
    {
        public string? mes { get; set; }
        public double o_vencido { get; set; }
        public double o_porvencer { get; set; }
        public double o_activa { get; set; }
        public double r_vencido { get; set; }
        public double r_porvencer { get; set; }
        public double r_activa { get; set; }
        public double total_operacion { get; set; }
        public double total_revolvente { get; set; }
        public double total_vencido { get; set; }
        public double total_porvencer { get; set; }
        public double total_activa { get; set; }
        public double total_recuperado { get; set; }
    }
}
