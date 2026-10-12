namespace HD_Cobranza.Modelos.Dashboard.Dash_Indicadores
{
    public class mdl_Dashboard_GestionCobranza
    {
        public string? estado { get; set; }
        public double total { get; set; }
        public int clientes_objetivo { get; set; }
        public double recuperado { get; set; }
        public int clientes_recuperados { get; set; }
        public double conveniado { get; set; }
        public int clientes_conveniados { get; set; }
        public float gestionado { get; set; }
        public float gestionado_pagado {  get; set; }
        public float gestionado_nopagado {  get; set; }
        public int clientes_gestionados {  get; set; }
        public int clientes_gestionados_pagados {  get; set; }
        public int clientes_gestionados_nopagados { get; set; }
        public double pendiente { get; set; }
        public int clientes_pendientes { get; set; }
        public double total_objetivo { get; set; }
    }
}
