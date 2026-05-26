using System.ComponentModel.DataAnnotations;

namespace HD_Auditoria.Modelos.Conteo_Piezas
{
    public class mdl_Agregar_Nueva_Pieza
    {
        public string folio { get; set; }

        [Required(ErrorMessage = "La familia es un valor requerido")]
        [StringLength(50, ErrorMessage = "La familia no puede exceder los 50 caracteres")]
        public string familia { get; set; }
        [Required(ErrorMessage = "El código es un valor requerido")]
        [StringLength(50, ErrorMessage = "El código no puede exceder los 50 caracteres")]
        public string codigo { get; set; }
        [Required(ErrorMessage = "La descripción es un valor requerido")]
        [StringLength(200, ErrorMessage = "La descripción no puede exceder los 200 caracteres")]
        public string descripcion { get; set; }
        [Required(ErrorMessage = "La unidad de medida es un valor requerido")]
        public string unidad_medida { get; set; }
        [Required(ErrorMessage = "El costo unitario es un valor requerido")]
        public float costo_unitario { get; set; }
        [Required(ErrorMessage = "La cantidad es un valor requerido")]
        public float conteo { get; set; }
        [Required(ErrorMessage = "La ubicación es un valor requerido")]
        [StringLength(30, ErrorMessage = "La ubicación no puede exceder los 30 caracteres")]
        public string posicion { get; set; }
        public int? id_auditor { get; set; }
    }
}
