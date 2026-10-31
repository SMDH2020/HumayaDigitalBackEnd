namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_HistorialEstado
    {
        public long HistorialEstadoId { get; set; }
        public long SolicitudId { get; set; }

        public int? EstadoAnteriorId { get; set; }
        public string? EstadoAnteriorCodigo { get; set; }
        public string? EstadoAnteriorNombre { get; set; }

        public int EstadoNuevoId { get; set; }
        public string EstadoNuevoCodigo { get; set; } = "";
        public string EstadoNuevoNombre { get; set; } = "";

        public int? UsuarioCambioId { get; set; }
        public string? UsuarioCambioNombre { get; set; }
        public DateTime FechaCambio { get; set; }
        public string? Motivo { get; set; }
        public string Origen { get; set; } = "";
    }
}
