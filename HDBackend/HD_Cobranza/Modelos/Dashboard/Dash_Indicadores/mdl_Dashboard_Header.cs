namespace HD_Cobranza.Modelos.Dashboard.Dash_Indicadores
{
    public class mdl_Dashboard_Header
    {
        public float importe_total {  get; set; }
        public int clientes_total { get; set; }
        public float importe_objetivo {  get; set; }
        public int clientes_objetivo { get; set; }
        public float importe_recuperado {  get; set; }
        public int clientes_recuperados { get; set; }
        public float porc_importe_recuperado { get; set; }
        public float porc_clientes_recuperados { get; set; }
    }
}
