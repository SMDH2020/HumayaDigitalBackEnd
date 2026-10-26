using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    // Input para agregar una funcionalidad sin una solicitud real de origen
    // (o de otro proyecto) directo al backlog de un mes/anio, desde la
    // pantalla de Backlog Mensual. El SP la agrega a la solicitud
    // administrativa "Solicitudes Extras {Anio} - {AreaSolicitante}"
    // (se crea la primera vez que se usa esa combinacion).
    public class mdl_TI_FuncionalidadExtraCrear
    {
        [Required(ErrorMessage = "El area solicitante es un valor requerido")]
        [StringLength(300, ErrorMessage = "El area solicitante no puede exceder 300 caracteres")]
        public string AreaSolicitante { get; set; } = "";

        [Range(2000, 2100, ErrorMessage = "El anio no es valido")]
        public short Anio { get; set; }

        [Required(ErrorMessage = "El nombre de la funcionalidad es un valor requerido")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "La descripcion es un valor requerido")]
        public string Descripcion { get; set; } = "";

        [Range(typeof(decimal), "0", "9999999", ErrorMessage = "Los dias estimados no pueden ser negativos")]
        public decimal? DiasEstimados { get; set; }

        [Range(1, 12, ErrorMessage = "El mes programado debe estar entre 1 y 12")]
        public byte MesProgramado { get; set; }

        [Range(2000, 2100, ErrorMessage = "El anio programado no es valido")]
        public short AnioProgramado { get; set; }
    }
}
