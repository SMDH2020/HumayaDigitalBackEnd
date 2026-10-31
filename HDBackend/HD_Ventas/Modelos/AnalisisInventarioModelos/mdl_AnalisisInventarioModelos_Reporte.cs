namespace HD_Ventas.Modelos.AnalisisInventarioModelos
{
    /// <summary>
    /// Una fila por modelo. Los nombres de las propiedades son exactamente los de las columnas
    /// del SP (idGeneral conserva su mayuscula). Las columnas numericas nunca vienen NULL.
    /// </summary>
    public class mdl_AnalisisInventarioModelos_Reporte
    {
        // NULL si el modelo no esta registrado en el catalogo (Ventas.Generales)
        public int? idGeneral { get; set; }
        public string? modelo { get; set; } = "";
        // NULL si no esta registrado. Se deja como texto: en el catalogo la linea es string.
        public string? linea { get; set; }

        public int objetivo_anual { get; set; }
        public int venta_comparativo_corte { get; set; }
        public int venta_actual_corte { get; set; }
        public int dif_corte { get; set; }
        public int venta_comparativo_resto { get; set; }
        public int objetivo_resto { get; set; }
        public int dif_objetivo_resto { get; set; }
        public int inventario_actual { get; set; }
        public int meses_inventario { get; set; }
        public int pedidos_resto { get; set; }
        public int diferencia_stock { get; set; }

        // Pedidos por surtir con entrega en el periodo (periodo_fin + n)
        public int pedidos_mes_1 { get; set; }
        public int pedidos_mes_2 { get; set; }
        public int pedidos_mes_3 { get; set; }
        public int pedidos_mes_4 { get; set; }
        public int pedidos_mes_5 { get; set; }
        public int pedidos_mes_6 { get; set; }
        public int pedidos_mes_7 { get; set; }
        public int pedidos_mes_8 { get; set; }
        public int pedidos_mes_9 { get; set; }
        public int pedidos_mes_10 { get; set; }
        public int pedidos_mes_11 { get; set; }

        // Objetivo (venta estimada) del periodo (periodo_fin + n)
        public int objetivo_mes_1 { get; set; }
        public int objetivo_mes_2 { get; set; }
        public int objetivo_mes_3 { get; set; }
        public int objetivo_mes_4 { get; set; }
        public int objetivo_mes_5 { get; set; }
        public int objetivo_mes_6 { get; set; }
        public int objetivo_mes_7 { get; set; }
        public int objetivo_mes_8 { get; set; }
        public int objetivo_mes_9 { get; set; }
        public int objetivo_mes_10 { get; set; }
        public int objetivo_mes_11 { get; set; }

        // 1 = el modelo esta en Generales, 0 = no esta registrado
        public int en_catalogo { get; set; }

        // NULL si esta en catalogo; si no, texto tipo "No registrado en Modelos, pero aparece en: ..."
        public string? advertencia { get; set; }
    }
}
