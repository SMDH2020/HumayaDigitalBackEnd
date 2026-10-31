using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    // Input del formulario de edicion de los datos base de una solicitud ya
    // registrada. El SP rechaza la operacion si la solicitud ya tiene una
    // version de alcance APROBADA.
    public class mdl_TI_SolicitudEditar
    {
        [Range(1, long.MaxValue, ErrorMessage = "La solicitud es un valor requerido")]
        public long SolicitudId { get; set; }

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
    }
}
