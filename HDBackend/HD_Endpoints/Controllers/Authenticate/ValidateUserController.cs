using HD.Generales.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Authenticate
{
    public class ValidateUserController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ValidateUserController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Get()
        {
            {
                string CadenaConexion = Configuracion["ConnectionStrings:Login"];
                AD_ValidateUser datos = new AD_ValidateUser(CadenaConexion);
                var result = await datos.UsuarioSesion(Sesion.usuario());

                return Ok(new { usuario = result.usuario, modulos = result.modulos, menus = result.menus, presas=result.presas});
            }
        }
    }
}
