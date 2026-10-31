using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_CambioProgramacionResolver
    {
        [Range(1, long.MaxValue, ErrorMessage = "El CambioProgramacionId es un valor requerido")]
        public long CambioProgramacionId { get; set; }

        [Required(ErrorMessage = "El estado de resolucion es un valor requerido")]
        public string EstadoResolucion { get; set; } = "";   // APROBADO | RECHAZADO

        public string? ComentarioResolucion { get; set; }
    }
}
