using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos.PedidosJD
{
    public class mdl_PedidosJD_Detalle_Linea
    {
        [Required(ErrorMessage = "El unit_code de la linea es un valor requerido")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El unit_code de la linea debe tener 2 caracteres")]
        public string? unit_code { get; set; }

        [Required(ErrorMessage = "El pedido de la linea es un valor requerido")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El pedido de la linea debe tener 6 caracteres")]
        public string? pedido { get; set; }

        [Required(ErrorMessage = "El numero de linea (n) es un valor requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El numero de linea (n) debe ser mayor o igual a 1")]
        public int? n { get; set; }

        public string? codigo { get; set; }

        // Llega como string y se guarda como texto
        public string? cantidad { get; set; }

        public string? descripcion { get; set; }
        public string? disponible { get; set; }
    }
}
