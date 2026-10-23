namespace HD_GestionProyectosTI.Modelos
{
    // Fila de dbo.TI_VersionesAlcance (detalle o listado).
    public class mdl_TI_VersionAlcance
    {
        public long VersionAlcanceId { get; set; }
        public long SolicitudId { get; set; }
        public int NumeroVersion { get; set; }
        public long? VersionAnteriorId { get; set; }
        public string? Objetivo { get; set; }
        public string? AlcanceIncluido { get; set; }
        public string? AlcanceExcluido { get; set; }
        public string? Supuestos { get; set; }
        public string? Dependencias { get; set; }
        public string EstadoVersion { get; set; } = "";
        public int CreadaPorUsuarioId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaEnvioAprobacion { get; set; }
        public DateTime? FechaResolucion { get; set; }
        public string? ObservacionesResolucion { get; set; }

        // Derivado con SUM() sobre las funcionalidades activas en
        // sp_TI_VersionesAlcance_Obtener -- nunca un total capturado a mano.
        // NULL si ninguna funcionalidad activa tiene DiasEstimados aun.
        public decimal? TotalDiasEstimados { get; set; }

        // Solo se llena en el detalle (sp_TI_VersionesAlcance_Obtener), via QueryMultiple.
        public List<mdl_TI_Funcionalidad>? Funcionalidades { get; set; }
    }
}
