using HD_AccesoDatos.ADAuthenticate;
using HD_Generales.Autenticate;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;

namespace HDEndpoints.Controllers
{
    public class UpdatePasswordController : MyBase
    {
        [HttpPost]
        public async Task<IHttpActionResult> Post(mdlAUpdatePassword update)
        {
            if (ModelState.IsValid)
            {
                string CadenaConexion = ConfigurationManager.ConnectionStrings["ConexionHDLogin"].ConnectionString;
                // Aquí realizarías la autenticación del usuario (por ejemplo, verificar las credenciales en una base de datos)
                // Si el usuario es válido, puedes generar un token JWT y devolverlo en la respuesta
                ADLogin datos = new ADLogin(CadenaConexion);
                var result = await datos.ActualizarContraseña(update);
                return Ok(new { mensaje = result });
            }
            else
            {
                return BadRequest(ModelState);
            }

        }
    }
}