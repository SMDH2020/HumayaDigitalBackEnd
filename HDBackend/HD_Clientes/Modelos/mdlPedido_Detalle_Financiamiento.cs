using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlPedido_Detalle_Financiamiento
    {

        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression(@"^[SC0-9]+$", ErrorMessage = "El campo folio debe estar formado solo por caracteres numericos e iniciales SC")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo folio debe estar formado por 13 digitos")]
        public string? folio { get; set; }

        [Required(ErrorMessage = "El campo registro es un valor requerido")]
        [Range(-1, 20, ErrorMessage = "El campo registro esta fuera de rango")]
        public short  docto { get; set; }
        public DateTime vencimiento { get; set; }
        public string? svencimiento => vencimiento.ToString("dd-MM-yyyy");
        public string? dvencimiento => vencimiento.ToString("yyyy-MM-dd");

        [Required(ErrorMessage = "El campo dias es un valor requerido")]
        [Range(0, 1000, ErrorMessage = "El campo dias esta fuera de rango")]
        public short dias { get; set; }

        [Required(ErrorMessage = "El campo taza es un valor requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo cantidad esta fuera de rango")]
        public double importefinanciar { get; set; }

        [Required(ErrorMessage = "El campo taza es un valor requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo cantidad esta fuera de rango")]
        public double tasa { get; set; }

        [Required(ErrorMessage = "La campo interes es un valor requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo interes esta fuera de rango")]
        public double interes { get; set; }

        [Required(ErrorMessage = "El campo total a pagar es un valor requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "El campo total a pagar esta fuera de rango")]
        public double totalpagar { get; set; }
        public string? usuario { get; set; }
    }
}
