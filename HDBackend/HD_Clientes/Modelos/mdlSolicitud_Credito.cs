using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito
    {

        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression(@"^(SC[0-9]+|-999)$", ErrorMessage = "El campo folio debe estar formado por 'SC' seguido de números o ser '-999'")]
        public string folio { get; set; } = "";


        [Required(ErrorMessage = "El idcliente es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo idcliente debe estar formado  por numeros")]
        public int idcliente { get; set; }

        [Required(ErrorMessage = "El Tipo de Solicitud es un valor requerido")]
        [RegularExpression(@"^[A|I|O|J|E]+$", ErrorMessage = "El campo Tipo de Solicitud debe estar formado por las siguientes opciones [A][I][O][J][E]")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "El campo Tipo de Solicitud debe estar formado por 1 digito")]
        public string tipo_solicitud { get; set; } = "";

        [Required(ErrorMessage = "El Importe es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Importe debe estar formado por numeros")]
        public double importe { get; set; }

        public char estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
