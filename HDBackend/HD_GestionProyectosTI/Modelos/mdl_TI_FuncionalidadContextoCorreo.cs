namespace HD_GestionProyectosTI.Modelos
{
    // Fila de salida de sp_TI_Funcionalidades_ContextoCorreo: lo necesario
    // para armar el correo de "funcionalidad priorizada" o "enviada a
    // revision" (solicitud + solicitante ya resueltos).
    public class mdl_TI_FuncionalidadContextoCorreo
    {
        public long FuncionalidadId { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = "";
        public byte? MesProgramado { get; set; }
        public short? AnioProgramado { get; set; }

        public long SolicitudId { get; set; }
        public string Folio { get; set; } = "";
        public string Titulo { get; set; } = "";
        public int SolicitanteUsuarioId { get; set; }
        public string? SolicitanteNombre { get; set; }
        public string? SolicitanteCorreo { get; set; }
    }
}
