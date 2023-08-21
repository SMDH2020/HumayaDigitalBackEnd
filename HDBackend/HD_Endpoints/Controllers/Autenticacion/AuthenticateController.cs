using Microsoft.AspNetCore.Mvc;
using HD.Generales.Autenticate;
using HD.Generales.Consultas;
using HD.Notificaciones.Autenticacion;
using HD.Security.clases;
namespace HD.Endpoints.Controllers.Autenticacion
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Post(mdlLogin login)
        {
            string _cadenaconexion = "Data Source=192.168.0.51; Initial Catalog=HumayaDigital_Usuarios; integrated security=false; user=HDLogin; password=!M4qu1n4r1a*SQL=2023?";
            AD_Autenticacion auth = new AD_Autenticacion(_cadenaconexion);
            mdlSesionresult result = await auth.Autenticar(login);
            if (auth.Valido)
            {
                await NE_Auth_CodigoSeguridad.enviar(result.autenticacion.email, result.autenticacion.codigoautenticacion);
                GenerarJWT jwt = new GenerarJWT();
                string token = jwt.CrearToken(result.id, null, "", "");
                result
            }
        }
    }
}