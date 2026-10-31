using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_AprobacionPrioridad
    {
        [Range(1, long.MaxValue, ErrorMessage = "El identificador de la decision es un valor requerido")]
        public long DecisionPrioridadId { get; set; }

        [Required(ErrorMessage = "El tipo de aprobador es un valor requerido")]
        public string TipoAprobador { get; set; } = "";   // JEFATURA_TI | GERENCIA_PLANEACION

        [Required(ErrorMessage = "El estado de la decision es un valor requerido")]
        public string EstadoDecision { get; set; } = "";  // APROBADA | RECHAZADA

        public string? Comentario { get; set; }
    }
}
