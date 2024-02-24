using HD.Generales.Autenticate;
using HD.Generales.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Authenticate
{
    public class UsuarioMenuController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public UsuarioMenuController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idusuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_UsuarioMenu_Listado datos = new AD_UsuarioMenu_Listado(CadenaConexion);
            var result = await datos.Listado(idusuario);
            return Ok(result);

        }
    }
}
