using HD.Security;
using HD_Dashboard.Consultas.Vendedor;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Dashboard
{
    public class VendScorecardController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public VendScorecardController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetScorecard()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            VenScorecard datos = new VenScorecard(CadenaConexion);
            string usuario = Sesion.usuario();
            var result = await datos.Listado(usuario);
            return Ok(result);

        }
    }
}
