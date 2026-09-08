using System.ComponentModel.DataAnnotations;

namespace Postventa.Modelos
{
    public class mdl_Paquetes_Mantenimiento
    {
        public int id_paquete {  get; set; }

        [Required(ErrorMessage = "El nombre es un valor requerido")]
        [RegularExpression(@"^[. , - # $ % ñ Ñ a-zA-Z0-9]+$", ErrorMessage = "El campo nombre debe contener solo letras y numeros")]
        public string paquete { get; set; }

        [Required(ErrorMessage = "La periocidad es un valor requerido")]
        public int periocidad {  get; set; }
        public DateTime fecha {  get; set; }


        [Required(ErrorMessage = "El contenido es un valor requerido")]
        [RegularExpression(@"^[. , - # $ % ñ Ñ a-zA-Z0-9]+$", ErrorMessage = "El campo contenido debe contener solo letras y numeros")]
        public string contenido { get; set; }
    }
}
