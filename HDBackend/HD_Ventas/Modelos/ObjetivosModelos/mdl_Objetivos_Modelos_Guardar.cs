using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos.ObjetivosModelos
{
    public class mdl_Objetivos_Modelos_Guardar : IValidatableObject
    {
        [Range(2024, 2100, ErrorMessage = "El ejercicio debe ser un año válido (2024 en adelante).")]
        public int ejercicio { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La sucursal debe ser mayor a 0.")]
        public int idsucursal { get; set; }

        // Solo vienen las celdas que el usuario modifico, no todo el ejercicio
        [Required(ErrorMessage = "La lista de objetivos es un valor requerido.")]
        [MinLength(1, ErrorMessage = "Debe enviar al menos un objetivo.")]
        public List<mdl_Objetivos_Modelos_Guardar_Detalle>? objetivos { get; set; }

        // No se permite la misma celda (idgeneral + periodo) dos veces en el mismo envio
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (objetivos == null)
                yield break;

            var repetidos = objetivos
                .Where(o => o != null)
                .GroupBy(o => new { o.idgeneral, o.periodo })
                .Where(g => g.Count() > 1)
                .Select(g => $"modelo {g.Key.idgeneral} periodo {g.Key.periodo}")
                .ToList();

            if (repetidos.Count > 0)
            {
                yield return new ValidationResult(
                    $"Hay objetivos repetidos en la lista: {string.Join(", ", repetidos)}.",
                    new[] { nameof(objetivos) });
            }
        }
    }
}
