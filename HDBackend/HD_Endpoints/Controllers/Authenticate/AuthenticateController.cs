using HD.Generales.Autenticate;
using HD.Generales.Consultas;
using HD.Notifications.Autentication;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Authenticate
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController:ControllerBase
    {
        private readonly IConfiguration Configuracion;

        public AuthenticateController(IConfiguration configuration)
        {
            Configuracion = configuration;
        }
        [HttpPost]
        public async Task<ActionResult>Post(mdlLogin mdl)
        {
            if (mdl is null)
            {
                return BadRequest("Error en datos enviados");
            }
            if (ModelState.IsValid)
            {
                string CadenaConexion = Configuracion["ConnectionStrings:Login"];

                 AD_Autenticacion datos = new AD_Autenticacion(CadenaConexion);
                var result = await datos.Autenticar(mdl);

                string? email = result.autenticacion?.email;
                string? codigoautenticacion = result.autenticacion?.codigoautenticacion;

                if (email == null) email= string.Empty;
                if (codigoautenticacion == null) codigoautenticacion = string.Empty;
                await NE_Auth_CodigoSeguridad.enviar(email , codigoautenticacion);

                string iussuer = Configuracion["Jwt:Issuer"];
                string audience = Configuracion["Jwt:Audience"];
                string securitytkey = Configuracion["Jwt:Login"];

                string? usuario = result.sesion?.idusuario;
                if(usuario == null) usuario = string.Empty;

                var token = await JwtManager.GenerarTocken(usuario, usuario, securitytkey, iussuer, audience,15);
                return Ok(new { usuario = result.sesion, token });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
