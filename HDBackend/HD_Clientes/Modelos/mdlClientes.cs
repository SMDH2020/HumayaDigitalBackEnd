using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlClientes
    {
        public int idcliente { get; set; }

        [Required(ErrorMessage = "El RFC es un valor requerido")]
        [StringLength(13, MinimumLength = 12, ErrorMessage = "El campo RFC debe estar formado por 12 digitos personas morales y 13 personas fisicas")]
        [RegularExpression(@"^[A-Z0-9Ñ]+$", ErrorMessage = "El campo RFC debe estar formado solo por caracteres alfanuméricos")]

        public string? rfc { get; set; } = "";

        [Required(ErrorMessage = "La Razon Social es un valor requerido")]
        [RegularExpression(@"^[0-9 a-zA-ZÑ]+$", ErrorMessage = "El campo Razon Social debe estar formado solo por caracteres alfabeticos")]
        public string? razon_social { get; set; }

        [Required(ErrorMessage = "El Tipo de Persona es un valor requerido")]
        [RegularExpression(@"^[MF]+$", ErrorMessage = "El campo Tipo Persona debe estar formado por las siguientes opciones [M][F]")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "El campo Tipo de Persona debe estar formado por 1 digitos")]
        public string? tipo_persona { get; set; }

        [Required(ErrorMessage = "El Medio de Contacto es un valor requerido")]
        [RegularExpression(@"^[LWHCOVSMN]+$", ErrorMessage = "El campo Medio de contacto debe estar formado por las siguientes opciones [LL][WH][CO][VS][MN]")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El campo Medio de Contacto debe estar formado por 2 digitos")]
        public string? medio_contacto { get; set; }

        [Required(ErrorMessage = "El tiempo de agricultor es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Tiempo de Agricultor debe estar formado solo por numeros")]
        public int tiempo_agricultor { get; set; } = 0;

        [Required(ErrorMessage = "La Agrupacion es un valor requerido")]
        [RegularExpression(@"^[AIG]+$", ErrorMessage = "El campo Agrupacion debe estar formado por las siguientes opciones [A][I][G]")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "El campo Medio de Contacto debe estar formado por 1 digitos")]
        public string agrupacion { get; set; } = "I";

        [Required(ErrorMessage = "El Regimen Fiscal es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Regimen Fiscal debe estar formado solo por numeros")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El campo Regimen Fiscal debe estar formado por 3 digitos")]
        public string? regimen_fiscal { get; set; }

        [Required(ErrorMessage = "El Tipo de Venta es un valor requerido")]
        [RegularExpression(@"^[COR]+$", ErrorMessage = "El campo Tipo de Venta debe estar formado por las siguientes opciones [CO][CR]")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El campo Tipo de Venta debe estar formado por 2 digitos")]
        public string tipo_venta { get; set; } = "CR";

        public bool estatus { get; set; } = true;


        public string? usuario { get; set; } = "";
        public string? referencia { get; set; } = "";
    }
}
