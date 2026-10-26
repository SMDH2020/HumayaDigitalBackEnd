using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos.InventarioModelo
{
    public class mdl_InventarioModelo_Excluir
    {
        [Required(ErrorMessage = "El modelo es un valor requerido")]
        [StringLength(100, ErrorMessage = "El modelo no puede exceder 100 caracteres")]
        public string? modelo { get; set; }

        [Required(ErrorMessage = "La accion es un valor requerido")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "La accion debe ser un solo caracter")]
        public string? accion { get; set; }
    }
}
