using System.ComponentModel.DataAnnotations;

namespace HD.Generales.Autenticate
{
    public class mdlLogin
    {
        [Required(ErrorMessage ="El usuario es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El usuario contiene caracteres invalidos")]
        [Range(1,99999,ErrorMessage ="Los caracteres se encuentran fuera de los limites")]
        public int user { get; set; }

        [Required(ErrorMessage ="El password es un valor requerido")]
        [RegularExpression(@"^[A-Za-z0-9!@#$%^&*(),.?:{}|<>]+$",ErrorMessage ="Existen caracteres no permitidos")]
        [MaxLength(20,ErrorMessage ="Solo se permiten 20 caracteres")]
        [MinLength(8,ErrorMessage ="El minimo de caracteres para la contraseña es de 8 caracteres")]
        public string password { get; set; }="";
    }
}