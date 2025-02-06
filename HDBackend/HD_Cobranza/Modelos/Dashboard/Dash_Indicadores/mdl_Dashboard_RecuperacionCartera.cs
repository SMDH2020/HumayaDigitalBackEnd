namespace HD_Cobranza.Modelos.Dashboard.Dash_Indicadores
{
    public class mdl_Dashboard_RecuperacionCartera
    {
        public string tipo_cartera {  get; set; }
        public float objetivo { get; set; }
        public float recuperado {  get; set; }
        public float porcentaje { get; set; }
        public string indicador { get; set; }
    }
}
