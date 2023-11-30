using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlClientes_Datos_Persona_Fisica : mdlClientes
    {
        [Required(ErrorMessage = "El Nombre es un valor requerido")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El campo Nombre debe estar formado por letras")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe de contener una longitud maxima de 50 caracteres")]
        public string? nombre { get; set; }

        [Required(ErrorMessage = "El Nombre es un valor requerido")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El campo Nombre debe estar formado por letras")]
        [StringLength(50, ErrorMessage = "El campo Nombre debe de contener una longitud maxima de 50 caracteres")]
        public string? nombre2 { get; set; }

        [Required(ErrorMessage = "El Apellido Paterno es un valor requerido")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El campo Apellido Paterno debe estar formado por letras")]
        [StringLength(50, ErrorMessage = "El campo Apellido Paterno debe de contener una longitud maxima de 50 caracteres")]
        public string? apellido_paterno { get; set; }

        [Required(ErrorMessage = "El Apellido Materno es un valor requerido")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "El campo Apellido Materno debe estar formado por letras")]
        [StringLength(50, ErrorMessage = "El campo Apellido Materno debe de contener una longitud maxima de 50 caracteres")]
        public string? apellido_materno { get; set; }

        [Required(ErrorMessage = "El CURP es un valor requerido")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "El campo CURP debe estar formado por caracteres alfanumericos")]
        [StringLength(18, MinimumLength = 18, ErrorMessage = "El campo CURP debe estar formado por 18 digitos")]
        public string? curp { get; set; }

        [Required(ErrorMessage = "El Sexo es un valor requerido")]
        [RegularExpression(@"^[MH]+$", ErrorMessage = "El campo metodo de pago debe estar formado por las siguientes opciones [M][H]")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El campo metodo de pago debe estar formado por 3 digitos")]
        public string? sexo { get; set; }

        [Required(ErrorMessage = "El Estado Civil es un valor requerido")]
        [RegularExpression(@"^[SOCA]+$", ErrorMessage = "El campo Estado Civil debe estar formado por las siguientes opciones [SO][CA]")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El campo Estado Civil debe estar formado por 3 digitos")]
        public string? estado_civil { get; set; }

        [Required(ErrorMessage = "La Edad es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Edad debe estar formado  por numeros")]
        [StringLength(2, ErrorMessage = "El campo Edad admite como maximo 2 digitos")]
        public int edad { get; set; }

        [Required(ErrorMessage = "El Regimen Conyugal es un valor requerido")]
        [RegularExpression(@"^[NABMS]+$", ErrorMessage = "El campo Regimen Conyugal debe estar formado por las siguientes opciones [NA][BM][BS]")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El campo Regimen Conyugal debe estar formado por 2 digitos")]
        public string regimen_conyugal { get; set; } = "NA";

    }
}
