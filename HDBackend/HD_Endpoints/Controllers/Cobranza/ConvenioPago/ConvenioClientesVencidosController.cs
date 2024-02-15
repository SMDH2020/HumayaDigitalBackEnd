using HD.Security;
using HD_Cobranza.Capturas.ConvenioPago;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza.ConvenioPago
{
    public class ConvenioClientesVencidosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ConvenioClientesVencidosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ClientesVencidos()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADClientesVencidos ad = new ADClientesVencidos(CadenaConexion);
            var result = await ad.ClientesVencidos();
            return Ok(result);

        }
    }
}
