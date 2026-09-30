using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos
{
    public class mdl_Modificar_Cotizacion
    {
        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression(@"^[RCT0-9]+$", ErrorMessage = "El campo folio debe estar formado solo por caracteres numericos e iniciales CT")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo folio debe estar formado por 13 digitos")]
        public string folio {  get; set; }

        [Required(ErrorMessage = "El asesor es un valor requerido")]
        public int idasesor { get; set; }
        public int crm { get; set; }

        [Required(ErrorMessage = "El tipo de pago es un valor requerido")]
        public string tipo_pago { get; set; }
        public string fase {  get; set; }

        [Required(ErrorMessage = "La vigencia es un valor requerido")]
        public string vigencia { get; set; }
        public int usuario { get; set; }


        [Required(ErrorMessage = "El detalle es un valor requerido")]
        public string detalle { get; set; }
    }
}
