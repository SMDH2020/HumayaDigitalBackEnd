namespace HD_GestionProyectosTI.Modelos
{
    // Detalle de solo lectura de una funcionalidad, para el modal de "ver
    // detalle" de Backlog Mensual (BacklogTI/Mensual no trae estos campos,
    // solo lo minimo para la tabla de backlog).
    public class mdl_TI_FuncionalidadDetalle
    {
        public long FuncionalidadId { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public string? ReglasNegocio { get; set; }
        public string? CriteriosAceptacion { get; set; }
        public string? Complejidad { get; set; }
        public decimal? DiasEstimados { get; set; }
    }
}
