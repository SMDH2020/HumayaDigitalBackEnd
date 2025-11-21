
using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos
{
    public class mdl_Agregar_Modelo
    {
        [Required(ErrorMessage = "La linea es un valor requerido")]
        public int idlinea {  get; set; }

        [Required(ErrorMessage = "El modelo es un valor requerido")]
        [RegularExpression(@"^[. , ( ) á é í ó ú # $ % ñ Ñ a-zA-Z0-9]+$", ErrorMessage = "El campo descripcion debe contener solo letras y numeros")]
        [StringLength(100, ErrorMessage = "El modelo no debe exceder los 100 caracteres")]
        public string modelo {  get; set; }

        [Required(ErrorMessage = "La descripcion es un valor requerido")]
        [RegularExpression(@"^[. , ( ) á é í ó ú # $ % ñ Ñ a-zA-Z0-9]+$", ErrorMessage = "El campo descripcion debe contener solo letras y numeros")]
        [StringLength(100, ErrorMessage = "La descripción no debe exceder los 100 caracteres")]
        public string descripcion_mdl {  get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El campo costo de refacciones esta fuera de rango")]
        public float costo_refacciones { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El campo costo de servicio esta fuera de rango")]
        public float costo_servicio { get; set; }

        [Required(ErrorMessage = "El precio de lista es un valor requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El campo precio de lista esta fuera de rango")]
        public float precio_lista {  get; set; }
        public string moneda {  get; set; }
        public int categoria { get; set; }
        public int usuario { get; set; }

        [RegularExpression(@"^[ .,#\$%ñÑa-zA-Z0-9áéíóúÁÉÍÓÚ()\\\[\]""]+$", ErrorMessage = "El campo caracteristicas debe contener solo letras, números y ciertos símbolos")]
        [StringLength(2500, ErrorMessage = "Las caracteristicas no debe exceder los 2500 caracteres")]
        public string caracteristicas {  get; set; }
        public string imagenes { get; set; }
    }
}
