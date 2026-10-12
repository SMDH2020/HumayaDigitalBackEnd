namespace HD_GestionProyectosTI.Modelos
{
    // Representa una fila de dbo.Solicitudes tal como la regresan los SPs
    // de listado/detalle (incluye folio y horas calculadas).
    public class mdl_Solicitud
    {
        public int idsolicitud { get; set; }
        public string? folio { get; set; }

        public string? tipo { get; set; }
        public string? titulo { get; set; }
        public string? descripcion { get; set; }
        public int idmodulo { get; set; }
        public int idusuario_solicitante { get; set; }

        public string? pregunta_informacion { get; set; }
        public string? pregunta_objetivo_negocio { get; set; }
        public string? pregunta_decisiones { get; set; }
        public string? pregunta_frecuencia { get; set; }
        public string? pregunta_uso_compartido { get; set; }

        public bool impacto_control_interno { get; set; }
        public bool impacto_normativo { get; set; }
        public bool impacto_financiero { get; set; }
        public string? impacto_comentario { get; set; }

        public string? estado { get; set; }
        public string? prioridad { get; set; }
        public string? priorizado_con { get; set; }
        public string? comentario_priorizacion { get; set; }

        public int rondas_rechazo_alcance { get; set; }

        public DateTime? fecha_estimada { get; set; }
        public DateTime? fecha_comprometida { get; set; }

        public DateTime fecha_creacion { get; set; }
        public DateTime fecha_actualizacion { get; set; }
        public int creado_por { get; set; }
        public int? actualizado_por { get; set; }

        // Calculadas por el SP a partir de Actividades
        public decimal horas_totales { get; set; }
        public decimal horas_terminadas { get; set; }
        public decimal avance_pct => horas_totales == 0 ? 0 : Math.Round(horas_terminadas / horas_totales * 100, 1);

        // Se llena solo en el detalle (sp_Solicitudes_Obtener)
        public List<mdl_Actividad>? actividades { get; set; }
    }
}
