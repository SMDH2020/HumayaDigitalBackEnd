using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos.PedidosJD
{
    public class mdl_PedidosJD_Lista_Guardar : IValidatableObject
    {
        [Required(ErrorMessage = "La cuenta es un valor requerido")]
        public string? cuenta { get; set; }

        [Required(ErrorMessage = "La fecha de consulta es un valor requerido")]
        public DateTimeOffset? fecha_consulta { get; set; }

        [Required(ErrorMessage = "La lista de pedidos es un valor requerido")]
        [MinLength(1, ErrorMessage = "La lista de pedidos no puede estar vacia")]
        public List<mdl_PedidosJD_Pedido>? pedidos { get; set; }

        // No se permiten pedidos repetidos (misma clave unit_code + pedido) en el mismo envio
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (pedidos == null)
                yield break;

            var repetidos = pedidos
                .Where(p => p != null && !string.IsNullOrWhiteSpace(p.unit_code) && !string.IsNullOrWhiteSpace(p.pedido))
                .GroupBy(p => new { p.unit_code, p.pedido })
                .Where(g => g.Count() > 1)
                .Select(g => $"{g.Key.unit_code}-{g.Key.pedido}")
                .ToList();

            if (repetidos.Count > 0)
            {
                yield return new ValidationResult(
                    $"Hay pedidos repetidos en la lista: {string.Join(", ", repetidos)}",
                    new[] { nameof(pedidos) });
            }
        }
    }
}
