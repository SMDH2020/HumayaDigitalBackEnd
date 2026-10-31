using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    // Input: editar una funcionalidad ya registrada en una version de
    // alcance. sp_TI_Funcionalidades_Editar rechaza la operacion si la
    // version ya tiene una aprobacion registrada.
    public class mdl_TI_FuncionalidadEditar
    {
        [Range(1, long.MaxValue, ErrorMessage = "El FuncionalidadId es un valor requerido")]
        public long FuncionalidadId { get; set; }

        public string? Codigo { get; set; }

        [Required(ErrorMessage = "El nombre de la funcionalidad es un valor requerido")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "La descripcion de la funcionalidad es un valor requerido")]
        public string Descripcion { get; set; } = "";

        public string? ReglasNegocio { get; set; }

        [Required(ErrorMessage = "Los criterios de aceptacion son un valor requerido")]
        public string CriteriosAceptacion { get; set; } = "";

        public string? Complejidad { get; set; }

        [Range(typeof(decimal), "0", "9999999", ErrorMessage = "Los dias estimados no pueden ser negativos")]
        public decimal? DiasEstimados { get; set; }
    }
}
