using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos.AnalisisInventarioModelos
{
    public class mdl_AnalisisInventarioModelos_RelacionModelos_Guardar
    {
        [Required(ErrorMessage = "El modelo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El modelo no puede exceder 100 caracteres.")]
        public string? modelo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El idgeneral no es válido.")]
        public int idgeneral { get; set; }
    }
}
