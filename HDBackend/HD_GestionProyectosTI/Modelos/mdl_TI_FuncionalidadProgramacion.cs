namespace HD_GestionProyectosTI.Modelos
{
    // Fila de salida de sp_TI_Funcionalidades_ProgramacionVigente_ListadoPorSolicitud.
    public class mdl_TI_FuncionalidadProgramacion
    {
        public long FuncionalidadId { get; set; }
        public long VersionAlcanceId { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = "";
        public decimal? DiasEstimados { get; set; }
        public byte? MesProgramado { get; set; }
        public short? AnioProgramado { get; set; }

        // Revision/aprobacion vigente de la funcionalidad (ver
        // sp_TI_Funcionalidades_ResolverRevision). RevisionEstado viene
        // calculado por el SP: "EN_REVISION" / "APROBADA" / "RECHAZADA" / null.
        public long? RevisionCambioProgramacionId { get; set; }
        public string? RevisionEstado { get; set; }
        public int? RevisionSolicitadoPorUsuarioId { get; set; }
        public DateTime? RevisionFechaSolicitud { get; set; }
        public int? RevisionDecididoPorUsuarioId { get; set; }
        public DateTime? RevisionFechaDecision { get; set; }
        public string? RevisionComentarioResolucion { get; set; }
    }
}
