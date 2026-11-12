using HD_Ventas.Modelos.ObjetivosModelos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos.AnalisisInventarioModelos
{
    public class mdl_AnalisisInventarioModelos_ProyeccionPedidos_Guardar : IValidatableObject
    {
        //[Range(1, int.MaxValue, ErrorMessage = "La sucursal debe ser mayor a 0.")]
        //public int idsucursal { get; set; }

        // Solo vienen las celdas que el usuario modifico, no todo el ejercicio
        [Required(ErrorMessage = "La lista de proyeccion es un valor requerido.")]
        [MinLength(1, ErrorMessage = "Debe enviar al menos una proyeccion.")]
        public List<mdl_Proyeccion_Pedidos_Detalle>? proyeccion { get; set; }

        // No se permite la misma celda (idgeneral + ejercicio + periodo) dos veces en el mismo envio
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (proyeccion == null)
                yield break;

            var repetidos = proyeccion
                .Where(o => o != null)
                .GroupBy(o => new { o.idgeneral, o.ejercicio, o.periodo })
                .Where(g => g.Count() > 1)
                .Select(g => $"modelo {g.Key.idgeneral} ejercicio {g.Key.ejercicio} periodo {g.Key.periodo}")
                .ToList();

            if (repetidos.Count > 0)
            {
                yield return new ValidationResult(
                    $"Hay proyecciones repetidas en la lista: {string.Join(", ", repetidos)}.",
                    new[] { nameof(proyeccion) });
            }
        }
    }
}
