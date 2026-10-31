using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    // Input: agregar una funcionalidad a una version de alcance ya creada
    // (distinto de mdl_TI_FuncionalidadCrear, que va dentro del detalle al
    // crear la version). sp_TI_Funcionalidades_Agregar rechaza la operacion
    // si la version ya tiene una aprobacion registrada.
    public class mdl_TI_FuncionalidadAgregar
    {
        [Range(1, long.MaxValue, ErrorMessage = "El VersionAlcanceId es un valor requerido")]
        public long VersionAlcanceId { get; set; }

        public string? Codigo { get; set; }

        [Required(ErrorMessage = "El nombre de la funcionalidad es un valor requerido")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "La descripcion de la funcionalidad es un valor requerido")]
        public string Descripcion { get; set; } = "";

        public string? ReglasNegocio { get; set; }

        [Required(ErrorMessage = "Los criterios de aceptacion son un valor requerido")]
        public string CriteriosAceptacion { get; set; } = "";

        public byte PrioridadInterna { get; set; } = 3;
        public string? Complejidad { get; set; }
        public int Orden { get; set; }

        [Range(typeof(decimal), "0", "9999999", ErrorMessage = "Los dias estimados no pueden ser negativos")]
        public decimal? DiasEstimados { get; set; }
    }
}
