using HD.Security;
using HD_Helpers.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Generales
{
    public class SucursalesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SucursalesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_HELP_Sucursales datos = new AD_HELP_Sucursales(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);

        }
    }
}
