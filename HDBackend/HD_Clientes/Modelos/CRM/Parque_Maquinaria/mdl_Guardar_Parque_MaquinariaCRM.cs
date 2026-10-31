using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos.CRM.Parque_Maquinaria
{
    /// <summary>
    /// Body del alta / edicion de una maquina del parque de maquinaria del cliente.
    /// idrelacion = -99 da de alta un registro nuevo; mayor a cero edita el existente.
    /// El usuario lo asigna el controller desde la sesion.
    /// </summary>
    public class mdl_Guardar_Parque_MaquinariaCRM : IValidatableObject
    {
        /// <summary>Valor que indica que el registro es un alta nueva.</summary>
        public const int ALTA_NUEVA = -99;

        [Range(1, int.MaxValue, ErrorMessage = "El Cliente es un valor requerido")]
        public int idcliente { get; set; }

        public int idrelacion { get; set; } = ALTA_NUEVA;

        [Required(ErrorMessage = "La Categoria es un valor requerido")]
        [StringLength(50, ErrorMessage = "El campo Categoria debe de contener una longitud maxima de 50 caracteres")]
        public string? categoria { get; set; }

        [Required(ErrorMessage = "El Tipo es un valor requerido")]
        [StringLength(50, ErrorMessage = "El campo Tipo debe de contener una longitud maxima de 50 caracteres")]
        public string? tipo { get; set; }

        [Required(ErrorMessage = "La Marca es un valor requerido")]
        [StringLength(100, ErrorMessage = "El campo Marca debe de contener una longitud maxima de 100 caracteres")]
        public string? marca { get; set; }

        [Required(ErrorMessage = "El Modelo es un valor requerido")]
        [StringLength(100, ErrorMessage = "El campo Modelo debe de contener una longitud maxima de 100 caracteres")]
        public string? modelo { get; set; }

        [Required(ErrorMessage = "La Serie es un valor requerido")]
        [StringLength(100, ErrorMessage = "El campo Serie debe de contener una longitud maxima de 100 caracteres")]
        public string? serie { get; set; }

        public int anio { get; set; }

        public string? comentarios { get; set; } = "";

        public int usuario { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (idrelacion != ALTA_NUEVA && idrelacion <= 0)
            {
                yield return new ValidationResult(
                    "El identificador del registro no es valido: use -99 para dar de alta o el id existente para editar",
                    new[] { nameof(idrelacion) });
            }

            int anioMaximo = DateTime.Now.Year + 1;
            if (anio < 1900 || anio > anioMaximo)
            {
                yield return new ValidationResult(
                    "El Anio debe estar entre 1900 y " + anioMaximo,
                    new[] { nameof(anio) });
            }
        }
    }
}
