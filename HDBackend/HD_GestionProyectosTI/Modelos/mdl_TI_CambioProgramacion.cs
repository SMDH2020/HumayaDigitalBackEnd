namespace HD_GestionProyectosTI.Modelos
{
    // Fila de salida de sp_TI_CambiosProgramacion_ListadoPorSolicitud.
    public class mdl_TI_CambioProgramacion
    {
        public long CambioProgramacionId { get; set; }
        public long FuncionalidadId { get; set; }
        public string? FuncionalidadCodigo { get; set; }
        public string? FuncionalidadNombre { get; set; }
        public long SolicitudId { get; set; }
        public byte? MesAnteriorProgramado { get; set; }
        public short? AnioAnteriorProgramado { get; set; }
        public byte MesPropuesto { get; set; }
        public short AnioPropuesto { get; set; }
        public int SolicitadoPorUsuarioId { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Motivo { get; set; } = "";
        public string EstadoCambio { get; set; } = "";
        public int? DecididoPorUsuarioId { get; set; }
        public DateTime? FechaDecision { get; set; }
        public string? ComentarioResolucion { get; set; }
    }
}
