using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    // Input del formulario de registro. SolicitanteUsuarioId y
    // CreadaPorUsuarioId los llena el controller desde ISesion -- nunca
    // viajan en el body aunque el cliente los mande.
    public class mdl_TI_SolicitudCrear
    {
        [Required(ErrorMessage = "El titulo es un valor requerido")]
        [StringLength(400, ErrorMessage = "El titulo no puede exceder 400 caracteres")]
        public string Titulo { get; set; } = "";

        [Required(ErrorMessage = "La descripcion de la necesidad es un valor requerido")]
        public string DescripcionNecesidad { get; set; } = "";

        public string? ProblemaActual { get; set; }
        public string? BeneficioEsperado { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El tipo de solicitud es un valor requerido")]
        public int TipoSolicitudId { get; set; }

        [Required(ErrorMessage = "El area solicitante es un valor requerido")]
        [StringLength(300, ErrorMessage = "El area solicitante no puede exceder 300 caracteres")]
        public string AreaSolicitante { get; set; } = "";

        public bool EsConfidencial { get; set; } = false;

        public int SolicitanteUsuarioId { get; set; }
        public int CreadaPorUsuarioId { get; set; }
    }
}
