using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_EvaluacionRegistrarDetalle
    {
        [Range(1, long.MaxValue, ErrorMessage = "El identificador de la evaluacion es un valor requerido")]
        public long EvaluacionPrioridadId { get; set; }

        public int CriterioPrioridadId { get; set; }

        [Range(1, 5, ErrorMessage = "La puntuacion debe estar entre 1 y 5")]
        public byte Puntuacion { get; set; }

        public string? Comentario { get; set; }
    }
}
