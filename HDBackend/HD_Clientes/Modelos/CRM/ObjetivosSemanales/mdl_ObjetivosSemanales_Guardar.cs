using System.ComponentModel.DataAnnotations;

namespace HD.Clientes.Modelos.CRM.ObjetivosSemanales
{
    /// <summary>
    /// Body del POST Guardar. El usuario lo asigna el controller desde la sesion.
    /// </summary>
    public class mdl_ObjetivosSemanales_Guardar
    {
        [Range(2000, 2999, ErrorMessage = "El Ejercicio es un valor requerido")]
        public int ejercicio { get; set; }

        [Range(1, 12, ErrorMessage = "El Periodo debe estar entre 1 y 12")]
        public int periodo { get; set; }

        [Required(ErrorMessage = "El detalle es un valor requerido")]
        [MinLength(1, ErrorMessage = "Debe enviar al menos un objetivo en el detalle")]
        public List<mdl_ObjetivosSemanales_Detalle> detalle { get; set; } = new List<mdl_ObjetivosSemanales_Detalle>();

        public int usuario { get; set; }
    }
}
