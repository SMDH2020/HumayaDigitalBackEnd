using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_EvaluacionDetalleItem
    {
        public int CriterioPrioridadId { get; set; }

        [Range(1, 5, ErrorMessage = "La puntuacion debe estar entre 1 y 5")]
        public byte Puntuacion { get; set; }

        public string? Comentario { get; set; }
    }
}
