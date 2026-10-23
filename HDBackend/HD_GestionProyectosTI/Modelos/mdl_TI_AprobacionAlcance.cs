using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    // El SP valida que TipoAprobador corresponda realmente a la relacion
    // del usuario (SolicitanteUsuarioId / rol JEFATURA_TI) -- este valor es
    // lo que el cliente dice que esta haciendo, no una autorizacion por si solo.
    public class mdl_TI_AprobacionAlcance
    {
        [Range(1, long.MaxValue, ErrorMessage = "El identificador de la version de alcance es un valor requerido")]
        public long VersionAlcanceId { get; set; }

        [Required(ErrorMessage = "El tipo de aprobador es un valor requerido")]
        public string TipoAprobador { get; set; } = "";   // SOLICITANTE | JEFATURA_TI

        [Required(ErrorMessage = "El estado de la decision es un valor requerido")]
        public string EstadoDecision { get; set; } = "";  // APROBADA | RECHAZADA

        public string? Comentario { get; set; }
    }
}
