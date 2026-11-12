namespace HD_Ventas.Modelos.AnalisisInventarioModelos
{
    public class mdl_AnalisisInventarioModelos_Reporte_View
    {
        // Result set 1: una fila por modelo
        public IEnumerable<mdl_AnalisisInventarioModelos_Reporte> modelos { get; set; } = new List<mdl_AnalisisInventarioModelos_Reporte>();

        // Result set 2: detalle por modelo, ejercicio y periodo (se liga por llave)
        public IEnumerable<mdl_AnalisisInventarioModelos_Reporte_Detalle> detalle { get; set; } = new List<mdl_AnalisisInventarioModelos_Reporte_Detalle>();
    }
}
