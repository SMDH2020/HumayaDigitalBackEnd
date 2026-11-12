namespace HD_Ventas.Modelos.AnalisisInventarioModelos
{
    /// <summary>
    /// Primer result set del reporte: una fila por modelo. Los nombres de las propiedades son exactamente
    /// los de las columnas del SP (idGeneral conserva su mayuscula). El detalle mes por mes ya no va en
    /// columnas fijas: viene en el segundo result set (mdl_AnalisisInventarioModelos_Reporte_Detalle),
    /// ligado por la columna llave.
    /// </summary>
    public class mdl_AnalisisInventarioModelos_Reporte
    {
        public string? llave { get; set; } = "";

        // NULL si el modelo no esta registrado en el catalogo (Ventas.Generales)
        public int? idGeneral { get; set; }
        public string? modelo { get; set; } = "";
        // NULL si no esta registrado. Se deja como texto: en el catalogo la linea es string.
        public string? linea { get; set; }

        public int objetivo { get; set; }
        public int venta_comparativo_corte { get; set; }
        public int venta_actual_corte { get; set; }
        public int dif_corte { get; set; }
        public int venta_comparativo_resto { get; set; }
        public int objetivo_resto { get; set; }
        public int dif_objetivo_resto { get; set; }
        public int inventario_actual { get; set; }
        public int inventario_proyeccion { get; set; }

        // NULL cuando no se puede calcular (por ejemplo, sin ventas)
        public decimal? meses_inventario { get; set; }

        public int pedidos_resto { get; set; }
        public int diferencia_stock { get; set; }

        // 1 = el modelo esta en Generales, 0 = no esta registrado
        public int en_catalogo { get; set; }

        // NULL si esta en catalogo; si no, texto tipo "No registrado en Modelos, pero aparece en: ..."
        public string? advertencia { get; set; }
    }
}
