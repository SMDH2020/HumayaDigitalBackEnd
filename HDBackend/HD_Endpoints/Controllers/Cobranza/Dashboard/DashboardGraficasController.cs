using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Capturas.Dashboard;
using HD_Cobranza.Reportes;
using HD_Reporteria;
using HD_Reporteria.Cobranza;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza.Dashboard
{
    public class DashboardGraficasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public DashboardGraficasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerInfoGraficas(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Graficas datos = new AD_Dashboard_Graficas(CadenaConexion);
            var result = await datos.ObtenerGraficas(ejercicio, periodo);
            return Ok(result);
        }

    }
}
