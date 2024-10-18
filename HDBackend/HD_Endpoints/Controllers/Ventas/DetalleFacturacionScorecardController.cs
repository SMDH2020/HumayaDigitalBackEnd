using HD.Security;
using HD_Ventas.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class DetalleFacturacionScorecardController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public DetalleFacturacionScorecardController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Mensual(string linea, int ejercicio, int periodo, string adr, string sucursal, string vendedor)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Facturacion_Scorecard_Mensual datos = new AD_Detalle_Facturacion_Scorecard_Mensual(CadenaConexion);
            var result = await datos.Get(linea, ejercicio, periodo, adr, sucursal, vendedor);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Acumulado(string linea, int ejercicioinicio, int periodoinicio, int ejerciciofin, int periodofin, string adr, string sucursal, string vendedor)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Facturacion_Scorecard_Acumulado datos = new AD_Detalle_Facturacion_Scorecard_Acumulado(CadenaConexion);
            var result = await datos.Get(linea, ejercicioinicio, periodoinicio, ejerciciofin, periodofin, adr, sucursal, vendedor);
            return Ok(result);
        }
    }
}
