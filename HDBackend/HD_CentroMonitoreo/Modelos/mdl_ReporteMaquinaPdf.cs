using System.ComponentModel.DataAnnotations;

namespace HD_CentroMonitoreo.Modelos
{
    // Parámetros para generar el reporte PDF de una máquina en un periodo (flujo n8n).
    public class mdl_ReporteMaquinaPdf
    {
        [Required(ErrorMessage = "Favor de indicar la máquina.")]
        [RegularExpression(@"^[A-Za-z0-9_-]{1,50}$", ErrorMessage = "La máquina seleccionada no es válida.")]
        public string jd_machine_id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Favor de seleccionar la fecha inicial.")]
        public DateTime? desde { get; set; }

        [Required(ErrorMessage = "Favor de seleccionar la fecha final.")]
        public DateTime? hasta { get; set; }
    }
}
