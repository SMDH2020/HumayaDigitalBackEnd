using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Capturas.Dashboard;
using HD_Cobranza.Reportes;
using HD_Reporteria;
using HD_Reporteria.Cobranza;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza.Dashboard
{
    public class DashboardReportesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public DashboardReportesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteGraficaTotal(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaTotal(ejercicio, periodo, tipo_grafica, tipo_cartera, estado_cartera, responsable_cobranza);
            return Ok(result);
        }
    }
}
