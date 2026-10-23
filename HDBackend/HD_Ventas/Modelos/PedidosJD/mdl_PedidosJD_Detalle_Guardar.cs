using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos.PedidosJD
{
    public class mdl_PedidosJD_Detalle_Guardar
    {
        [Required(ErrorMessage = "La cuenta es un valor requerido")]
        [StringLength(20, ErrorMessage = "La cuenta no puede exceder 20 caracteres")]
        public string? cuenta { get; set; }

        [Required(ErrorMessage = "La fecha de consulta es un valor requerido")]
        public DateTimeOffset? fecha_consulta { get; set; }

        // Lista oficial de los pedidos del lote (un objeto por pedido, maximo 100)
        [Required(ErrorMessage = "La lista de cabeceras es un valor requerido")]
        [MinLength(1, ErrorMessage = "La lista de cabeceras no puede estar vacia")]
        [MaxLength(100, ErrorMessage = "La lista de cabeceras no puede tener mas de 100 pedidos por envio")]
        public List<mdl_PedidosJD_Detalle_Cabecera>? cabeceras { get; set; }

        // Pueden venir vacias
        public List<mdl_PedidosJD_Detalle_Linea>? lineas { get; set; } = new List<mdl_PedidosJD_Detalle_Linea>();
        public List<mdl_PedidosJD_Detalle_Comentario>? comentarios { get; set; } = new List<mdl_PedidosJD_Detalle_Comentario>();
    }
}
