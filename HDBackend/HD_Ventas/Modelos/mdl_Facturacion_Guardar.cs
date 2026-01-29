using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HD_Ventas.Modelos
{
    public class mdl_Facturacion_Guardar
    {
        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression(@"^[RCT0-9]+$", ErrorMessage = "El campo folio debe estar formado solo por caracteres numericos e iniciales CT")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo folio debe estar formado por 13 digitos")]
        public string folio_cotizacion { get; set; }

        [Required(ErrorMessage = "La fecha de entrega es un valor requerido")]
        public string fecha_entrega { get; set; }

        [Required(ErrorMessage = "Entregado es un valor requerido")]
        [RegularExpression("^[SN]$", ErrorMessage = "El campo entregado debe ser S o N")]
        [StringLength(1, MinimumLength = 1, ErrorMessage = "El campo folio debe estar formado por 1 valor")]
        public string entregado { get; set; }

        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression(@"^[SC0-9]+$", ErrorMessage = "El campo folio debe estar formado solo por caracteres numericos e iniciales SC")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo folio debe estar formado por 13 digitos")]
        public string folio_solicitud { get; set; }

        [Required(ErrorMessage = "El contacto servicio es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo contacto servicio debe estar formado solo por caracteres numericos")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El campo folio debe estar formado por 10 digitos")]
        public string contacto_servicio { get; set; }

        [Required(ErrorMessage = "El contacto refacciones es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo contacto refacciones debe estar formado solo por caracteres numericos")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El campo folio debe estar formado por 10 digitos")]
        public string contacto_refacciones { get; set; }
        public string?  fase { get; set; }
        public int idcliente { get; set; }

        public string? usuario { get; set; }

    }
}
