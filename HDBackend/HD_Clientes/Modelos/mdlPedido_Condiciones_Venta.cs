using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos
{
    public class mdlPedido_Condiciones_Venta
    {
        [Required(ErrorMessage = "El folio es un valor requerido")]
        [RegularExpression(@"^[SC0-9]+$", ErrorMessage = "El campo folio debe estar formado solo por caracteres numericos e iniciales SC")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo folio debe estar formado por 13 digitos")]
        public string? folio { get; set; }

        [Required(ErrorMessage = "El condiciones es un valor requerido")]
        [RegularExpression(@"^[ a-zA-Z0-9]+$", ErrorMessage = "El campo condiciones debe contener solo letras y numeros")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "El campo condiciones admite como maximo 200 caracteres")]
        public string? condiciones { get; set; }

        [Required(ErrorMessage = "El observaciones es un valor requerido")]
        [RegularExpression(@"^[ a-zA-Z0-9]+$", ErrorMessage = "El campo observaciones debe contener solo letras y numeros")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "El campo observaciones admite como maximo 200 caracteres")]
        public string? observaciones { get; set; }

        [Required(ErrorMessage = "El deposito es un valor requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo deposito esta fuera de rango")]
        public double deposito { get; set; }

        [Required(ErrorMessage = "La taza es un valor requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo taza esta fuera de rango")]
        public double taza { get; set; }

        [Required(ErrorMessage = "El anticipo es un valor requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo anticipo esta fuera de rango")]
        public double anticipo { get; set; }

        [Required(ErrorMessage = "El plazo es un valor requerido")]
        public string? plazo { get; set; }

        [Required(ErrorMessage = "El campo mhusa o jdf es un valor requerido")]
        public string? mhusajdf { get; set; }

        [Required(ErrorMessage = "El campo gastos es un valor requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo gastos esta fuera de rango")]
        public double gastos { get; set; }

        [Required(ErrorMessage = "El campo enganche es un valor requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo enganche esta fuera de rango")]
        public double enganche { get; set; }
        public string? usuario { get; set; }
    }
}
