using HD.Generales.Autenticate;
using HD.Generales.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Authenticate
{
    public class CodigoSeguridadController:MyBase
    {
        private readonly IConfiguration Configuracion;
        public CodigoSeguridadController(IConfiguration configuration)
        {

            Configuracion = configuration;
        }

        [HttpPost]
        public async Task<ActionResult> Post(mdlCodigoSeguridad Login)
        {
            if (Login is null)
            {
                return BadRequest("Error en datos enviados");
            }
            if (ModelState.IsValid)
            {
                string CadenaConexion = Configuracion["ConnectionStrings:Login"];
                AD_UsuarioSesion datos = new AD_UsuarioSesion(CadenaConexion);
                var result = await datos.UsuarioSesion(Login);

                if (result.modulos.Count() == 0 || result.menus.Count() == 0)
                {
                    return BadRequest(new { mensaje = "No cuenta con permisos para acceder a la aplicación, favor de comunicarse con el administrador del sistema" });
                }
                string iussuer = Configuracion["Jwt:Issuer"];
                string audience = Configuracion["Jwt:Audience"];
                string securitytkey = Configuracion["Jwt:Login"];
                var token =await JwtManager.GenerarTocken(Login.usuario, Login.usuario, securitytkey, iussuer, audience,10080);
                return Ok(new { usuario = result.usuario, modulos = result.modulos, menus = result.menus, token });

            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
