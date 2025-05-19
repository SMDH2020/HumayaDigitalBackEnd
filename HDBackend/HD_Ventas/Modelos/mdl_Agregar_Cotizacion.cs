using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos
{
    public class mdl_Agregar_Cotizacion
    {
        public int idcliente { get; set; }

        [Required(ErrorMessage = "La razon social es un valor requerido")]
        [RegularExpression(@"^[. , # $ % ñ Ñ a-zA-Z0-9]+$", ErrorMessage = "El campo condiciones debe contener solo letras y numeros")]
        public string razon_social { get; set; }

        [Required(ErrorMessage = "El asesor es un valor requerido")]
        public int idasesor { get; set; }

        [Required(ErrorMessage = "El tipo de pago es un valor requerido")]
        public string tipo_pago { get; set; }

        [Required(ErrorMessage = "La vigencia es un valor requerido")]
        public string vigencia { get; set; }
        public int usuario { get; set; }

        [Required(ErrorMessage = "El detalle es un valor requerido")]
        public string detalle { get; set; }
    }
}
