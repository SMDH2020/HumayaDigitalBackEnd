using HD_CentroMonitoreo.Modelos.Dashboard;

namespace HD_CentroMonitoreo.Consultas.Dashboard
{
    /// <summary>
    /// Calcula el periodo anterior: el mismo numero de periodos inmediatamente antes del inicio.
    /// Ej: oct 2026 -> sep 2026; ene-oct 2026 (10 periodos) -> mar-dic 2025.
    /// </summary>
    public static class Dashboard_PeriodoAnterior
    {
        public static mdl_Dashboard_Periodo Calcular(mdl_Dashboard_Filtro filtro)
        {
            // Meses absolutos (ejercicio * 12 + mes base 0) para restar sin cuidar cruces de año.
            int inicio = filtro.ejercicioinicio * 12 + (filtro.periodoinicio - 1);
            int fin = filtro.ejerciciofin * 12 + (filtro.periodofin - 1);
            int cantidad = fin - inicio + 1;

            int anteriorFin = inicio - 1;
            int anteriorInicio = inicio - cantidad;

            return new mdl_Dashboard_Periodo
            {
                ejercicio_inicio = anteriorInicio / 12,
                periodo_inicio = anteriorInicio % 12 + 1,
                ejercicio_fin = anteriorFin / 12,
                periodo_fin = anteriorFin % 12 + 1
            };
        }
    }
}
