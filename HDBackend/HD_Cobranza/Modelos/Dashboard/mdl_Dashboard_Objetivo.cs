namespace HD_Cobranza.Modelos.Dashboard
{
    public class mdl_Dashboard_Objetivo
    {
        public string? tipo_cartera {  get; set; }
        public double saldo { get; set; }
        public double objetivo { get; set; }
        public double recuperado { get; set; }
        public double total {  get; set; }
        public double porcentaje {  get; set; }
        public int totalclientes {  get; set; }
    }
}
