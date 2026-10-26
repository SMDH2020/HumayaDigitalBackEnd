using System.ComponentModel.DataAnnotations;

namespace HD_GestionProyectosTI.Modelos
{
    public class mdl_TI_EstimacionCrear
    {
        [Range(1, long.MaxValue, ErrorMessage = "El identificador de la solicitud es un valor requerido")]
        public long SolicitudId { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "El identificador de la version de alcance es un valor requerido")]
        public long VersionAlcanceId { get; set; }

        public string? Riesgos { get; set; }
        public string? Supuestos { get; set; }
        public string? Dependencias { get; set; }
        public int? DiasCalendarioEstimados { get; set; }

        [MinLength(1, ErrorMessage = "Debe incluir el detalle de al menos una funcionalidad")]
        public List<mdl_TI_EstimacionDetalleItem> Detalle { get; set; } = new();

        public int RegistradaPorUsuarioId { get; set; }
    }
}
