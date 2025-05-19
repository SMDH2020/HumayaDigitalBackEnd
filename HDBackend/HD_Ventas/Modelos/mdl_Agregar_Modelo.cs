
using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos
{
    public class mdl_Agregar_Modelo
    {
        [Required(ErrorMessage = "La linea es un valor requerido")]
        public int idlinea {  get; set; }

        [Required(ErrorMessage = "El modelo es un valor requerido")]
        [RegularExpression(@"^[. , - # $ % ñ Ñ a-zA-Z0-9]+$", ErrorMessage = "El campo modelo debe contener solo letras y numeros")]
        public string modelo {  get; set; }

        [Required(ErrorMessage = "La descripcion es un valor requerido")]
        [RegularExpression(@"^[. , - # $ % ñ Ñ a-zA-Z0-9]+$", ErrorMessage = "El campo descripcion debe contener solo letras y numeros")]
        public string descripcion_mdl {  get; set; }

        [Required(ErrorMessage = "El precio de lista es un valor requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo precio de lista esta fuera de rango")]
        public float precio_lista {  get; set; }
        public int usuario { get; set; }
        public string caracteristicas {  get; set; }
        public string imagenes { get; set; }
    }
}
