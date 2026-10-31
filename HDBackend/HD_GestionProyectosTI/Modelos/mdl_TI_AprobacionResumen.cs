namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_AprobacionResumen
    {
        public string? TipoAprobador { get; set; }
        public string? EstadoAprobacion { get; set; }
        public int? AprobadorUsuarioId { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public DateTime? FechaDecision { get; set; }
        public string? Comentario { get; set; }
    }
}
