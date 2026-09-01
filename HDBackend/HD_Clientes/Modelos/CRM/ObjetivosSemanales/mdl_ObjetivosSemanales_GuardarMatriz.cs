using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos.CRM.ObjetivosSemanales
{
    /// <summary>
    /// Body del POST GuardarMatriz. El usuario lo asigna el controller desde la sesion.
    /// actualiza_vendedor = true sobrescribe los objetivos de TODOS los vendedores
    /// del ejercicio y periodo indicados. Es destructivo e irreversible.
    /// </summary>
    public class mdl_ObjetivosSemanales_GuardarMatriz : IValidatableObject
    {
        [Range(2000, 2999, ErrorMessage = "El Ejercicio es un valor requerido")]
        public int ejercicio { get; set; }

        [Range(1, 12, ErrorMessage = "El Periodo debe estar entre 1 y 12")]
        public int periodo { get; set; }

        public bool actualiza_vendedor { get; set; } = false;

        [Required(ErrorMessage = "La matriz es un valor requerido")]
        [MinLength(1, ErrorMessage = "Debe enviar al menos una linea en la matriz")]
        public List<mdl_ObjetivosSemanales_MatrizDetalle> matriz { get; set; } = new List<mdl_ObjetivosSemanales_MatrizDetalle>();

        public int usuario { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (matriz != null && matriz.Count > 0)
            {
                var repetidas = matriz.GroupBy(x => x.idlinea)
                                      .Where(g => g.Count() > 1)
                                      .Select(g => g.Key)
                                      .ToList();

                if (repetidas.Count > 0)
                {
                    yield return new ValidationResult(
                        "Existen lineas repetidas en la matriz: " + string.Join(", ", repetidas),
                        new[] { nameof(matriz) });
                }
            }
        }
    }
}
