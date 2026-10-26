using System.ComponentModel.DataAnnotations;

namespace HD_Ventas.Modelos.CodigosPedidosJD
{
    public class mdl_CodigosPedidosJD_Guardar
    {
        [Required(ErrorMessage = "El codigo es un valor requerido")]
        public string? codigo { get; set; }

        [Required(ErrorMessage = "La clave es un valor requerido")]
        public string? clave { get; set; }

        public string? descripcion { get; set; } = "";

        [Required(ErrorMessage = "El tipo es un valor requerido")]
        [RegularExpression("^(modelo|caracteristica)$", ErrorMessage = "El tipo debe ser 'modelo' o 'caracteristica'")]
        public string? tipo { get; set; }
    }
}
