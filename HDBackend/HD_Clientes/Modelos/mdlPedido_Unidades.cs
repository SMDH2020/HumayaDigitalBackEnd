using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlPedido_Unidades
    {
        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression(@"^[SC0-9]+$", ErrorMessage = "El campo folio debe estar formado solo por caracteres numericos e iniciales SC")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo folio debe estar formado por 13 digitos")]
        public string? folio { get; set; }

        [Required(ErrorMessage = "El campo registro es un valor requerido")]
        [Range(-1,20,ErrorMessage ="El campo registro esta fuera de rango")]
        public int registro { get; set; }

        [Required(ErrorMessage = "El campo nuevo o usado es un valor requerido")]
        public bool nuevo { get; set; }

        [Required(ErrorMessage = "El modelo es un valor requerido")]
        [RegularExpression(@"^[ a-zA-Z0-8]+$", ErrorMessage = "El campo modelo debe contener solo letras")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "El campo modelo admite como maximo 50 caracteres")]
        public string? modelo { get; set; }

        [Required(ErrorMessage = "La serie es un valor requerido")]
        [RegularExpression(@"^[ a-zA-Z0-8]+$", ErrorMessage = "El campo serie debe contener solo letras")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "El campo serie admite como maximo 50 caracteres")]
        public string? serie { get; set; }

        [Required(ErrorMessage = "La cantidad es un valor requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El campo cantidad esta fuera de rango")]
        public double cantidad { get; set; }

        [Required(ErrorMessage = "El precio es un valor requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El campo precio esta fuera de rango")]
        public double precio { get; set; }

        [Required(ErrorMessage = "El descuento es un valor requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El campo descuento esta fuera de rango")]
        public double descuento { get; set; }

        public string? usuario { get; set; }
    }
}
