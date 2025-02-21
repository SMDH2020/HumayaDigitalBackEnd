namespace HD_Ventas.Modelos
{
    public class mdlCarga_Scorecard_porVendedor_Dash
    {
        public string? linea { get; set; }
        public int objetivo { get; set; }
        public int unidades_vendidas { get; set; }
        public float importe {  get; set; }
        public float importe_proyectado { get; set; }
        public float porcentaje {  get; set; }
        public int objetivo_acumulado { get; set; }
        public int unidades_vendidas_acumulado { get; set; }
        public float importe_acumulado { get; set; }
        public float importe_proyectado_acumulado { get; set; }
        public float porcentaje_acumulado { get; set; }
    }
}
