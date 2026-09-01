using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Capturas.ConvenioPago;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza.ConvenioPago
{
    public class CPSaldosVencidosController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CPSaldosVencidosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Revolvente(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADVencimientosSaldos datos = new ADVencimientosSaldos(CadenaConexion);
            var result = await datos.ObtenerRevolvente(idcliente);
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Operacion(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADVencimientosSaldos datos = new ADVencimientosSaldos(CadenaConexion);
            var result = await datos.ObtenerOperacion(idcliente);
            return Ok(result);

        }
    }
}
