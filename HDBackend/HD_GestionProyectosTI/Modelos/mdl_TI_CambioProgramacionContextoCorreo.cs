namespace HD_GestionProyectosTI.Modelos
{
    // Fila de salida de sp_TI_CambiosProgramacion_ContextoCorreo: lo
    // necesario para armar el correo de "revision resuelta" (avisa a
    // Jefatura de TI cuando el solicitante aprueba o rechaza).
    public class mdl_TI_CambioProgramacionContextoCorreo
    {
        public long CambioProgramacionId { get; set; }
        public long FuncionalidadId { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = "";

        public long SolicitudId { get; set; }
        public string Folio { get; set; } = "";
        public string Titulo { get; set; } = "";
        public int SolicitanteUsuarioId { get; set; }
        public string? SolicitanteNombre { get; set; }
        public string? SolicitanteCorreo { get; set; }

        public string EstadoCambio { get; set; } = "";
        public string? ComentarioResolucion { get; set; }
    }
}
