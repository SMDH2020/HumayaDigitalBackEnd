using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlPedido_Datos_Generales
    {
        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression(@"^[SC0-9]+$", ErrorMessage = "El campo folio debe estar formado solo por caracteres numericos e iniciales SC")]
        [StringLength(13,MinimumLength =13,ErrorMessage ="El campo folio debe estar formado por 13 digitos")]
        public string? folio { get; set; }

        [Required(ErrorMessage = "El solicitante es un valor requerido")]
        [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "El campo solicitante debe contener solo letras")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El campo solicitante admite como maximo 50 caracteres")]
        public string? solicitante { get; set; }

        [Required(ErrorMessage = "El numero de celular es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo folio debe estar formado solo por caracteres numericos")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El campo folio debe estar formado por 10 digitos")]
        public string? celular { get; set; }

        [Required(ErrorMessage = "El correo electronico es un valor requerido")]
        public string? correoelectronico { get; set; }

        [Required(ErrorMessage = "La fecha de entrega es un valor requerido")]
        public DateTime fechaentrega { get; set; }

        [Required(ErrorMessage = "El domicilio es un valor requerido")]
        [RegularExpression(@"^[ A-Za-z0-9]+$", ErrorMessage = "El campo domicilio debe estar formado por letras y numeros")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El campo solicitante admite como maximo 100 caracteres")]
        public string? domicilio { get; set; }

        [Required(ErrorMessage = "El lugar de entrega es un valor requerido")]
        [RegularExpression(@"^[ A-Za-z0-9]+$", ErrorMessage = "El campo lugar de entrega debe estar formado por letras y numeros")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "El campo lugar de entrega admite como maximo 100 caracteres")]
        public string? lugarentrega { get; set; }

        [Required(ErrorMessage = "Las condiciones de credito son un valor requerido")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "El campo condiciones de crédito debe estar formado por letras y numeros")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "El campo condiciones de crédito admite como maximo 500 caracteres")]
        public string? condicionescredito { get; set; }


        [Required(ErrorMessage = "El metodo de pago es un valor requerido")]
        [RegularExpression(@"^[PUED]+$", ErrorMessage = "El campo metodo de pago debe estar formado por las siguientes opciones [PUE][PPD]")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El campo metodo de pago debe estar formado por 3 digitos")]
        public string? metodopago{ get; set; }

        [Required(ErrorMessage = "La forma de pago es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo forma de pago debe estar formado por 2 numeros")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El campo forma de pago debe estar formado por 2 digitos")]
        public string? formapago { get; set; }


        [Required(ErrorMessage = "El uso de CFDI es un valor requerido")]
        [RegularExpression(@"^[0-9GIP]+$", ErrorMessage = "El campo uso de cfdi debe estar formado por 3 numeros")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El campo uso de cfdi debe estar formado por 3 digitos")]
        public string? usocfdi { get; set; }


        [Required(ErrorMessage = "El tipo de relación es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo tipo de relación debe estar formado por 2 numeros")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El campo tipo de relación debe estar formado por 2 digitos")]
        public string? tiporelacion { get; set; }


        public string? anticipos { get; set; }

        public string? foliosanticipos { get; set; }


        public string? usuario { get; set; }
    }
}
