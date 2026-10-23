namespace HD_GestionProyectosTI.Modelos
{
    // Fila tal como la regresan sp_TI_Backlog_Mensual_Listado y
    // sp_TI_Backlog_Buscar: una funcionalidad con los datos de su solicitud
    // dueña ya resueltos (area, folio, titulo), para la pantalla de backlog
    // mensual. MesProgramado/AnioProgramado solo son relevantes en el
    // resultado de Buscar (indican donde esta programada actualmente, o
    // vienen en NULL si sigue sin programar).
    public class mdl_TI_BacklogFuncionalidad
    {
        public long FuncionalidadId { get; set; }
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = "";
        public decimal? DiasEstimados { get; set; }
        public byte? AvanceManual { get; set; }
        public byte? MesProgramado { get; set; }
        public short? AnioProgramado { get; set; }

        public long SolicitudId { get; set; }
        public string Folio { get; set; } = "";
        public string Titulo { get; set; } = "";
        public string? AreaSolicitante { get; set; }

        // Desarrolladores asignados (rol TI "DESARROLLADOR", puede haber
        // varios) -- formato "id:Nombre;id:Nombre", ya resuelto por el SP
        // (ver 27_BacklogDesarrolladoresYEliminarFuncionalidad.sql), para
        // que el frontend muestre los nombres y pre-marque el selector de
        // checkboxes con los ids actuales sin pedir nada aparte. Solo se
        // llena en sp_TI_Backlog_Mensual_Listado.
        public string? DesarrolladoresAsignados { get; set; }

        // Solo en el resultado de Buscar (sp_TI_Backlog_Buscar): estado de
        // revision vigente de la funcionalidad, mismo criterio que
        // PriorizacionFuncionalidadTI.js ("EN_REVISION"/"APROBADA"/"RECHAZADA").
        public long? RevisionCambioProgramacionId { get; set; }
        public string? RevisionEstado { get; set; }
    }
}
