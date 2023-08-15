using HD_AccesoDatos.ADAuthenticate;
using HD_Generales.Autenticate;
using HDNotificacionesEmail.Auth;
using HDSecurity;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HDEndpoints.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")] // tune to your needs
    public class AuthenticateController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> Post(mdlALogin Login)
        {
            if (Login is null)
            {
                return BadRequest("Error en datos enviados");
            }
            if (ModelState.IsValid)
            {
                string CadenaConexion = ConfigurationManager.ConnectionStrings["ConexionHDLogin"].ConnectionString;
                // Aquí realizarías la autenticación del usuario (por ejemplo, verificar las credenciales en una base de datos)
                // Si el usuario es válido, puedes generar un token JWT y devolverlo en la respuesta
                ADLogin datos = new ADLogin(CadenaConexion);
                var result = await datos.Autenticacion(Login);
                if (result == null)
                {
                    return BadRequest(datos.Mensaje);
                }
                await NECodigoSeguridad.enviar(result.autenticacion.email, result.autenticacion.codigoautenticacion);
                var token = JwtManager.GenerateToken(string.Concat(result.sesion.idusuario, "~", false.ToString()));
                return Ok(new { usuario = result.sesion, token });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<IHttpActionResult> Post(mdlALogin Login)
        //{
        //    if (Login is null)
        //    {
        //        return BadRequest("Error en datos enviados");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        string CadenaConexion = ConfigurationManager.ConnectionStrings["ConexionHDLogin"].ConnectionString;
        //        // Aquí realizarías la autenticación del usuario (por ejemplo, verificar las credenciales en una base de datos)
        //        // Si el usuario es válido, puedes generar un token JWT y devolverlo en la respuesta
        //        ADLogin datos = new ADLogin(CadenaConexion);
        //        var result = await datos.Autenticacion(Login);
        //        if (result == null)
        //        {
        //            return BadRequest(datos.Mensaje);
        //        }
        //        var token = JwtManager.GenerateToken(string.Concat(result.idusuario, '~', result.empleado));
        //        await NECodigoSeguridad.enviar(result.email, result.codigoautenticacion);
        //        result.codigoautenticacion = "0";
        //        result.email = "";
        //        return Ok(new { token, usuario = result });
        //    }
        //    else
        //    {
        //        return BadRequest(ModelState);
        //    }
        //}
    }
}