namespace HD_Cobranza.Modelos
{
    public class mdlResumenCarteraMensual
    {
        public string? mes { get; set; }
        public double o_vencido { get; set; }
        public double o_porvencer { get; set; }
        public double o_activa { get; set; }
        public double r_vencido { get; set; }
        public double r_porvencer { get; set; }
        public double r_activa { get; set; }
        public double total_vencido { get; set; }
        public double convenio { get; set; }
        public double vencido_recuperar { get; set; }
        public double total_porvencer { get; set; }
        public double total_recuperar { get; set; }
        public double por_vencido { get; set; }
        public double por_vencer { get; set; }
    }
}
