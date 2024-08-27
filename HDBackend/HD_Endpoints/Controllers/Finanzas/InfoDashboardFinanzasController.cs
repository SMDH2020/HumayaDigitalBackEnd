using HD_Finanzas.AccesoDatos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class InfoDashboardFinanzasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public InfoDashboardFinanzasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> Obtener_Dashboard()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_InfoDashboardFinanzas datos = new AD_InfoDashboardFinanzas(CadenaConexion);
            var result = await datos.GetDash();
            return Ok(result);
        }
    }
}
