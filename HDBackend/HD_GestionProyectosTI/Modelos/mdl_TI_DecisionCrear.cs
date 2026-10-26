using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_DecisionCrear
    {
        [Range(1, long.MaxValue, ErrorMessage = "El identificador de la solicitud es un valor requerido")]
        public long SolicitudId { get; set; }

        // Bloque 2: la decision ya no depende de una evaluacion de 6
        // criterios -- queda opcional, solo por trazabilidad si alguna
        // integracion antigua la sigue enviando. El flujo nuevo nunca la manda.
        public long? EvaluacionPrioridadId { get; set; }

        [Required(ErrorMessage = "El tipo de decision es un valor requerido")]
        public string TipoDecision { get; set; } = "";   // PRIORIZAR | MANTENER_BACKLOG | POSPONER | NO_PRIORIZAR | RECHAZAR

        public int? PrioridadId { get; set; }
        public int? PosicionBacklog { get; set; }

        [Required(ErrorMessage = "El motivo es un valor requerido")]
        public string Motivo { get; set; } = "";
    }
}
