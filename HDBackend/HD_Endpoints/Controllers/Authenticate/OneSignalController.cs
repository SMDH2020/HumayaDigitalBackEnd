using HD.Generales.Autenticate;
using HD.Generales.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Authenticate
{
    public class OneSignalController :MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public OneSignalController(IConfiguration configuration,ISesion sesion)
        {

            Configuracion = configuration;
            Sesion = sesion;
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
                if (Login.oneSignalID.Equals("")) return Ok(new { mensaje= "Cargado con exito"});
                string CadenaConexion = Configuracion["ConnectionStrings:Login"];
                AD_UsuarioSesion datos = new AD_UsuarioSesion(CadenaConexion);
                Login.usuario = Sesion.usuario();
                var result = await datos.OneSignal(Login);
                return Ok(result);

            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
