namespace HD_Ventas.Modelos.AnalisisInventarioModelos
{
    /// <summary>
    /// Segundo result set del reporte: una fila por modelo (llave), ejercicio y periodo.
    /// Se liga con mdl_AnalisisInventarioModelos_Reporte por la columna llave.
    /// </summary>
    public class mdl_AnalisisInventarioModelos_Reporte_Detalle
    {
        public string? llave { get; set; } = "";
        public int ejercicio { get; set; }
        public int periodo { get; set; }
        public int objetivo { get; set; }
        public int pedidos { get; set; }
        public int proyectado { get; set; }
    }
}
