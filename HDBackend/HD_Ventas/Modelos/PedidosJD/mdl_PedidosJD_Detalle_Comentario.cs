using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos.PedidosJD
{
    public class mdl_PedidosJD_Detalle_Comentario
    {
        [Required(ErrorMessage = "El unit_code del comentario es un valor requerido")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El unit_code del comentario debe tener 2 caracteres")]
        public string? unit_code { get; set; }

        [Required(ErrorMessage = "El pedido del comentario es un valor requerido")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El pedido del comentario debe tener 6 caracteres")]
        public string? pedido { get; set; }

        [Required(ErrorMessage = "El numero de comentario (n) es un valor requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El numero de comentario (n) debe ser mayor o igual a 1")]
        public int? n { get; set; }

        // "YYYY-MM-DD" o null
        public DateTime? fecha { get; set; }

        [StringLength(500, ErrorMessage = "El comentario no puede exceder 500 caracteres")]
        public string? comentario { get; set; }
    }
}
