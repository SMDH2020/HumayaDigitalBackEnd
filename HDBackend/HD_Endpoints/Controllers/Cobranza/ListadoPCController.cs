using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Reportes;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class ListadoPCController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ListadoPCController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Editar()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_ListadoPC datos = new ADCob_ListadoPC(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_ListadoPC datos = new ADCob_ListadoPC(CadenaConexion);
            var result = await datos.Listado();
            var docResult = await XLSCob_ListadoPagare.GenerarExcel(result);
            return Ok(docResult);

        }

    }
}
