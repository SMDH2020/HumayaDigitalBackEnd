using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    // motivo es obligatorio para RECHAZADA y CANCELADA; se valida en el
    // controller porque depende del valor de EstadoNuevoCodigo, no se puede
    // expresar con una sola DataAnnotation.
    public class mdl_TI_CambioEstado
    {
        [Range(1, long.MaxValue, ErrorMessage = "El identificador de la solicitud es un valor requerido")]
        public long SolicitudId { get; set; }

        [Required(ErrorMessage = "El estado nuevo es un valor requerido")]
        [StringLength(100)]
        public string EstadoNuevoCodigo { get; set; } = "";

        public string? Motivo { get; set; }
    }
}
