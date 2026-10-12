using HD.Security;
using HD_CentroMonitoreo.Consultas;
using HD_Cobranza.Capturas.ConvenioPago;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.CentroMonitoreo
{
    public class MensajesCentroSolucionesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public MensajesCentroSolucionesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerListadoMensajesCSC(int ejercicioinicio, int ejerciciofin, int periodoinicio, int periodofin, string adr, string sucursal, string estatus)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Mensajes datos = new AD_Mensajes(CadenaConexion);
            var result = await datos.MensajesCentroMonitoreo(ejercicioinicio, ejerciciofin, periodoinicio, periodofin, adr, sucursal, estatus);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDF(int orden, int organizacion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Mensajes datos = new AD_Mensajes(CadenaConexion);
            var result = await datos.GetArchivoPDF(orden, organizacion);
            return Ok(result);
        }
    }
}
