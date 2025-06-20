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

        public async Task<ActionResult> Obtener_Dashboard(int periodoinicio, int periodofin, int ejercicio, string adr, string sucursales)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_InfoDashboardFinanzas datos = new AD_InfoDashboardFinanzas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.GetDash(periodoinicio, periodofin, ejercicio, adr, sucursales, usuario);
            return Ok(result);
        }
    }
}
