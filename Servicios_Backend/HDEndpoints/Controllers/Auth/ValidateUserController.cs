using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;

namespace HDEndpoints.Controllers.Auth
{
    public class ValidateUserController : MyBase
    {
        private readonly IDependencyResolver _dependencyResolver;
        public ValidateUserController()
        {
            _dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;

        }
        [HttpPost]
        public async Task<IHttpActionResult> Post()
        {
            var result = _dependencyResolver.BeginScope();
            var usuario = UserSession;
            return Ok();
            //if (Login is null)
            //{
            //    return BadRequest("Error en datos enviados");
            //}
            //if (ModelState.IsValid)
            //{
            //    string CadenaConexion = ConfigurationManager.ConnectionStrings["ConexionHDLogin"].ConnectionString;
            //    // Aquí realizarías la autenticación del usuario (por ejemplo, verificar las credenciales en una base de datos)
            //    // Si el usuario es válido, puedes generar un token JWT y devolverlo en la respuesta
            //    ADLogin datos = new ADLogin(CadenaConexion);
            //    var result = await datos.UsuarioSesion(Login);
            //    if (result.usuario == null)
            //    {
            //        return BadRequest(datos.Mensaje);
            //    }
            //    if (result.modulos.Count() == 0 || result.menus.Count() == 0)
            //    {
            //        return BadRequest("No cuenta con permisos para acceder a la aplicación, favor de comunicarse con el administrador del sistema");
            //    }
            //    var token = JwtManager.GenerateToken(string.Concat(Login.usuario, "~", true.ToString()));
            //    return Ok(new { usuario = result.usuario, modulos = result.modulos, menus = result.menus, token });

            //}
            //else
            //{
            //    return BadRequest(ModelState);
            //}
        }
    }
}