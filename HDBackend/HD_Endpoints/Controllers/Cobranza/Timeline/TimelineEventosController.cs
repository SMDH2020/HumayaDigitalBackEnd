using HD.Security;
using HD_Cobranza.Capturas.Juridico;
using HD_Cobranza.Consultas.Juridico;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza.Timeline
{
    public class TimelineEventosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public TimelineEventosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> Timeline(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Timeline_Eventos_Obtener datos = new AD_Timeline_Eventos_Obtener(CadenaConexion);
            var result = await datos.Timeline(idcliente);
            return Ok(result);
        }
    }
}
