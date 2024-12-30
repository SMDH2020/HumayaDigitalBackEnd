namespace HD_Cobranza.Modelos.ReporteRecuperacionCompleta
{
    public class mdl_Recuperacion_Completa
    {
            public string? mes { get; set; }
            public double total_cartera { get; set; }
            public double cartera_activa { get; set; }
            public double cartera_porvencer { get; set; }
            public double cartera_vencida { get; set; }
            public double cartera_mes { get; set; }
            public double recuperacion_activa { get; set; }
            public double recuperacion_porvencer { get; set; }
            public double recuperacion_vencida { get; set; }
            public double recuperacion_mes { get; set; }
            public double total_recuperado { get; set; }
            public double objetivo { get; set; }
            public double objetivo_porvencer { get; set; }
            public double objetivo_vencido { get; set; }
            public double recuperado { get; set; }
            public double porc { get; set; }
            public string? indicador { get; set; }
            public double porcvencido { get; set; }
            public string? indicadorvencido { get; set; }
            public double porcporvencer { get; set; }
            public string? indicadorporvencer { get; set; }
        }
    }