using HD.Security;
using HD_Cobranza.Capturas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class RecuperacionCarteraMensualController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public RecuperacionCarteraMensualController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener(int ejercicio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADRecuperacionCarteraMensual datos = new ADRecuperacionCarteraMensual(CadenaConexion);

            var result = await datos.Obtener(ejercicio);
            return Ok(result);
        }
    }
}
