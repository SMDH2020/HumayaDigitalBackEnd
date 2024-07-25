using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlSolicitudCredito_Documentacion_View
    {
        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression(@"^[PSC0-9]+$", ErrorMessage = "El campo folio debe estar formado solo por caracteres numericos e iniciales SC")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo folio debe estar formado por 13 digitos")]
        public string? folio { get; set; }

        [Required(ErrorMessage = "El id del documento es un valor requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El campo iddocumento esta fuera de rango")]
        public int iddocumento { get; set; }

        [Required(ErrorMessage = "El documento es un valor requerido")]
        public string? documento { get; set; }

        public string? comentarios { get; set; }

        [Required(ErrorMessage = "La existencia es un valor requerido")]
        public string? extension { get; set; }

        [Required(ErrorMessage = "La vigencia es un valor requerido")]
        public string? vigencia { get; set; }

        public string? usuario { get; set; }
    }
}
