using HD.Security;
using HD_Dashboard.Consultas;
using HD_Dashboard.Consultas.Vendedor;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Dashboard
{
    public class SeleccionarScorecardController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SeleccionarScorecardController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> usuario()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Seleccionar_Scorecard datos = new AD_Seleccionar_Scorecard(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.usuario(usuario);
            return Ok(result);

        }
    }
}
