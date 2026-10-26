using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_FuncionalidadResolverRevision
    {
        [Range(1, long.MaxValue, ErrorMessage = "El CambioProgramacionId es un valor requerido")]
        public long CambioProgramacionId { get; set; }

        [Required(ErrorMessage = "El EstadoResolucion es un valor requerido")]
        public string EstadoResolucion { get; set; } = "";

        public string? ComentarioResolucion { get; set; }
    }
}
