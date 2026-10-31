using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_EvaluacionCrear
    {
        [Range(1, long.MaxValue, ErrorMessage = "El identificador de la solicitud es un valor requerido")]
        public long SolicitudId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "El identificador de la version de alcance es un valor requerido")]
        public long VersionAlcanceId { get; set; }

        // Las 6 puntuaciones son obligatorias desde la creacion -- no se
        // admite una evaluacion con criterios sin responder.
        [MinLength(1, ErrorMessage = "Debe incluir la puntuacion de los criterios activos")]
        public List<mdl_TI_EvaluacionDetalleItem> Detalle { get; set; } = new();

        public int EvaluadaPorUsuarioId { get; set; }
    }
}
