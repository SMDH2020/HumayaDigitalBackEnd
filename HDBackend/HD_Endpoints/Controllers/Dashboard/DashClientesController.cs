using HD.Security;
using HD_Dashboard.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Dashboard
{
    public class DashClientesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public DashClientesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Dashboard(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            Dash_Clientes_Main datos = new Dash_Clientes_Main(CadenaConexion);
            var result = await datos.Dashboard(idcliente);
            return Ok(result);

        }
    }
}
