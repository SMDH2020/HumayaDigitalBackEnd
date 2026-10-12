
using System.Data;

namespace HD_Cobranza.Modelos.Dashboard
{
    public class mdlProyeccionRecuperarResult
    {
        public IEnumerable<mdlProyeccionRecuperar>? listado { get; set; }
        public string? tipo_cartera { get; set; }
        public string? columnas { get; set; }
    }
}
