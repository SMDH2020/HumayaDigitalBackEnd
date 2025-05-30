using System.ComponentModel.DataAnnotations;

namespace Postventa.Modelos
{
    public class mdl_Mensaje_Garantia
    {
        [Required(ErrorMessage = "El mensaje es un valor requerido")]
        [RegularExpression(@"^[. , - # $ % ñ Ñ a-zA-Z0-9]+$", ErrorMessage = "El campo mensaje debe contener solo letras y numeros")]
        [StringLength(1000, ErrorMessage = "El modelo no debe exceder los 1000 caracteres")]
        public string mensaje { get; set; }
        public string inicio_vigencia { get; set; }
        public string vigencia { get; set; }
        public int usuario { get; set; }
    }
}
