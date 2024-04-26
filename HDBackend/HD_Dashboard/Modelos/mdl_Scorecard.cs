namespace HD_Dashboard.Modelos
{
    public class mdl_Scorecard
    {
        public int idvendedor { get; set; }
        public string? vendedor { get; set; }
        public double objetivo_tractores { get; set; }
        public double unidades_vendidas_tractores { get; set; }
        public double objetivo_tractores_porcentaje { get; set; }
        public double objetivo_tractores_acumulado { get; set; }
        public double unidades_vendidas_tractores_acumulado { get; set; }
        public double acumulado_tractores_porcentaje { get; set; }
        public double objetivo_implementos { get; set; }
        public double unidades_vendidas_implementos { get; set; }
        public double objetivo_implementos_porcentaje { get; set; }
        public double objetivo_implementos_acumulado { get; set; }
        public double unidades_vendidas_implementos_acumulado { get; set; }
        public double acumulado_implementos_porcentaje { get; set; }
        public double objetivo_usados { get; set; }
        public double unidades_vendidas_usados { get; set; }
        public double objetivo_usados_porcentaje { get; set; }
        public double objetivo_usados_acumulado { get; set; }
        public double unidades_vendidas_usados_acumulado { get; set; }
        public double acumulado_usados_porcentaje { get; set; }

        public double objetivo { get; set; }
    }
}
