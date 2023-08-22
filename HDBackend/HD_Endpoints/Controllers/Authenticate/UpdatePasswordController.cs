using HD.Generales.Autenticate;
using HD.Generales.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Authenticate
{
    public class UpdatePasswordController :MyBase
    {
        private readonly IConfiguration Configuracion;
        public UpdatePasswordController(IConfiguration configuration)
        {
            Configuracion = configuration;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlUpdatePassword update)
        {
            if (ModelState.IsValid)
            {
                string CadenaConexion = Configuracion["ConnectionStrings:Login"];
                AD_UpdatePassword datos = new AD_UpdatePassword(CadenaConexion);
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
