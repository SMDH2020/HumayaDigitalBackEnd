using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.Modelos.CotizacionesVentas
{
    public class mdl_Agregar_Cotizacion_Nuevo
    {
        public string folio {  get; set; }
        public int idcliente { get; set; }

        [Required(ErrorMessage = "La razon social es un valor requerido")]
        //[RegularExpression(@"^[. - , # $ % ñ Ñ a-zA-Z0-9]+$", ErrorMessage = "El campo condiciones debe contener solo letras y numeros")]
        public string razon_social { get; set; }

        [Required(ErrorMessage = "El asesor es un valor requerido")]
        public int idasesor { get; set; }

        [Required(ErrorMessage = "La vigencia es un valor requerido")]
        public string vigencia { get; set; }

        [Required(ErrorMessage = "El esquema de pago es un valor requerido")]
        public int idesquema { get; set; }

        [Required(ErrorMessage = "La moneda es un valor requerido")]
        public string moneda { get; set; }

        public int mostrar_precio_lista { get; set; }
        public int usuario { get; set; }


        [RegularExpression(@"^[. , # $ % ñ Ñ a-zA-Z0-9]+$", ErrorMessage = "El campo condiciones debe contener solo letras y numeros")]
        [StringLength(500, ErrorMessage = "El campo terminos y condiciones debe tener máximo 500 caracteres")]
        public string? terminos { get; set; }

        [Required(ErrorMessage = "El detalle es un valor requerido")]
        public string detalle { get; set; }
    }
}
