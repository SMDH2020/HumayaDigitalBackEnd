using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito_Otros_Ingresos
    {
        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression(@"^[SC0-9]+$", ErrorMessage = "El campo folio debe estar formado solo por caracteres numericos e iniciales SC")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo folio debe estar formado por 13 digitos")]
        public string folio { get; set; }

        [Required(ErrorMessage = "El Registro es un valor requerido")]
        [RegularExpression(@"^[0-9]|-1$", ErrorMessage = "El campo Registro debe estar formado solo por numeros")]
        public short registro { get; set; }

        [Required(ErrorMessage = "La Fuente es un valor requerido")]
        [RegularExpression(@"^[ A-Za-z0-9]+$", ErrorMessage = "El campo Fuente debe estar formado por letras")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "El campo Fuente admite como maximo 50 caracteres")]
        public string fuente { get; set; }


        [Required(ErrorMessage = "El Importe es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Importe debe estar formado por numeros")]
        public double ingresos { get; set; }

        public bool estatus { get; set; }

        public string? usuario { get; set; } = "";
    }
}
