using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Teletrabajo.Consultas;
using Teletrabajo.Modelos;

namespace HD.Endpoints.Controllers.Teletrabajo
{
    [ApiController]
    [Route("api/[controller]")]
    public class TEL_RegistroController : ControllerBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public TEL_RegistroController(IConfiguration configuration,
            ISesion sesion)

        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> acceso(TEL_mdl_InfoSesion mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Teletrabajo"];
            TEL_AD_RegistrarSesion datos = new TEL_AD_RegistrarSesion(CadenaConexion);
            var result = await datos.PrimerRegistro(mdl);



            string iussuer = Configuracion["Jwt:Issuer"];
            string audience = Configuracion["Jwt:Audience"];
            string securitytkey = Configuracion["Jwt:Login"];


            var token = await JwtManager.GenerarTocken(mdl.usuario.ToString(), "teletrabajo", securitytkey, iussuer, audience, 10080);
            
            
            return Ok(new { registros=result, token
            }); 

        }
    }
}
