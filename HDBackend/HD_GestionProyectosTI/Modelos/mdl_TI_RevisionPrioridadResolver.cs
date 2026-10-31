using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_RevisionPrioridadResolver
    {
        [Range(1, long.MaxValue, ErrorMessage = "El SolicitudId es un valor requerido")]
        public long SolicitudId { get; set; }

        [Required(ErrorMessage = "El estado de resolucion es un valor requerido")]
        public string EstadoResolucion { get; set; } = "";

        public string? Motivo { get; set; }
    }
}
