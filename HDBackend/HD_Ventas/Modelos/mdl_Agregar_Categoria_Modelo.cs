using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos
{
    public class mdl_Agregar_Categoria_Modelo
    {

        [Required(ErrorMessage = "La razon social es un valor requerido")]
        [RegularExpression(@"^[. , # $ % ñ Ñ a-zA-Z0-9]+$", ErrorMessage = "El campo descripcion debe contener solo letras y numeros")]
        public string descripcion { get; set; }
        public int usuario { get; set; }
    }
}
