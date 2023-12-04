using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlClientes_Domicilio
    {
        [Required(ErrorMessage = "El idcliente es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo idcliente debe estar formado solo por numeros")]
        public int idcliente { get; set; }

        [Required(ErrorMessage = "El Orden es un valor requerido")]
        [RegularExpression(@"^[0-9]|-1$", ErrorMessage = "El campo Orden debe estar formado solo por numeros")]
        public int orden { get; set; }

        [Required(ErrorMessage = "La idlocalidad es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo idlocalidad debe estar formado solo por numeros")]
        public int idlocalidad { get; set; }

        [Required(ErrorMessage = "La Direccion es un valor requerido")]
        [RegularExpression(@"^[ A-Za-z0-9]+$", ErrorMessage = "El campo direccion debe estar formado por letras y numeros")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "El campo direccion admite como maximo 200 caracteres")]
        public string direccion { get; set; }

        [Required(ErrorMessage = "El Tipo de Domicilio es un valor requerido")]
        [RegularExpression(@"^[FOC]+$", ErrorMessage = "El campo Tipo de Domicilio debe estar formado por las siguientes opciones [F][O][C]")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "El campo metodo de pago debe estar formado por 1 digitos")]
        public string tipodomicilio { get; set; }

        public bool principal { get; set; }

        [RegularExpression(@"^[ A-Za-z0-9]+$", ErrorMessage = "El campo Referencia1 debe estar formado por letras y números")]
        [StringLength(100, ErrorMessage = "El campo Referencia1 admite como máximo 100 caracteres")]
        public string referencia1 { get; set; }

        [RegularExpression(@"^[ A-Za-z0-9]+$", ErrorMessage = "El campo Referencia2 debe estar formado por letras y números")]
        [StringLength(100, ErrorMessage = "El campo Referencia2 admite como máximo 100 caracteres")]
        public string referencia2 { get; set; }

        public bool estatus { get; set; } = true;

        public string? usuario { get; set; } = "";
    }
}
