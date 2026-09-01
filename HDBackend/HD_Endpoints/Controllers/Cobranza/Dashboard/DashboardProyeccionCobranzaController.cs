using HD.Security;
using HD_Cobranza.Capturas.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza.Dashboard
{
    public class DashboardProyeccionCobranzaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public DashboardProyeccionCobranzaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerProyecciones(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Proyecciones datos = new AD_Dashboard_Proyecciones(CadenaConexion);
            var result = await datos.ObtenerProyecciones(ejercicio, periodo);
            return Ok(result);
        }

    }
}
