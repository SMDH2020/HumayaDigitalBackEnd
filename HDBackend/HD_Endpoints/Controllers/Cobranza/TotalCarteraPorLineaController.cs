using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Reportes;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class TotalCarteraPorLineaController :MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public TotalCarteraPorLineaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idsucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_TotalCarteraPorLinea datos = new ADCob_TotalCarteraPorLinea(CadenaConexion);
            var result = await datos.Listado(idsucursal);
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(int idsucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_TotalCarteraPorLinea datos = new ADCob_TotalCarteraPorLinea(CadenaConexion);
            var result = await datos.Listado(idsucursal);
            var docResult = await XLSCob_TotalCartera_Linea.CrearExcel(result);
            return Ok(docResult);

        }
    }
}
