using System.ComponentModel.DataAnnotations;

namespace Postventa.Modelos
{
    public class mdl_Agregar_Contacto_Servicios_Pendientes
    {
        public int id_registro {  get; set; }

        [Required(ErrorMessage = "El contenido es un valor requerido")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El campo solo debe contener números")]
        public string contacto { get; set; }
    }
}
