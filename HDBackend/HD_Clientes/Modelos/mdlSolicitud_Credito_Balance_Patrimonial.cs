using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlSolicitud_Credito_Balance_Patrimonial
    {

        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression(@"^[SC0-9]+$", ErrorMessage = "El campo folio debe estar formado solo por caracteres numericos e iniciales SC")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo folio debe estar formado por 13 digitos")]
        public string folio { get; set; }

        [Required(ErrorMessage = "El Registro es un valor requerido")]
        [RegularExpression(@"^[0-9]|-1$", ErrorMessage = "El campo Registro debe estar formado solo por numeros")]
        public short registro { get; set; }


        public string concepto { get; set; }

        [Required(ErrorMessage = "El Tipo es un valor requerido")]
        [RegularExpression(@"^[AC|AF|PC|PF]+$", ErrorMessage = "El campo Tipo debe estar formado por las siguientes opciones [AC][AF][PC][PF]")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El campo Tipo debe estar formado por 2 digitos")]
        public string tipo { get; set; }

        [Required(ErrorMessage = "El Importe es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo Importe debe estar formado por numeros")]
        public double importe { get; set; }

        public bool estatus { get; set; }

    
        public string? usuario { get; set; }
    }
}
