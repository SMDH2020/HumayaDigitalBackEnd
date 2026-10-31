using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Nomina.Consultas;

namespace HD.Endpoints.Controllers.Nomina
{
        public class DashboardNominaController : MyBase
        {
            private readonly IConfiguration Configuracion;
            private readonly ISesion Sesion;
            public DashboardNominaController(IConfiguration configuration, ISesion sesion)
            {
                Configuracion = configuration;
                Sesion = sesion;
            }

            [HttpGet]
            [Route("/api/[controller]/[action]")]
            public async Task<ActionResult> ObtenerDashboard()
            {
                string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
                AD_Dashboard datos = new AD_Dashboard(CadenaConexion);
                var result = await datos.obtenerDashboard();
                return Ok(result);
            }
        }
    
}
