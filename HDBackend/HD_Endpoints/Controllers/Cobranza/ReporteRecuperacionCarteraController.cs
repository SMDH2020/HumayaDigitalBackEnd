using HD_Cobranza.Modelos.ReporteRecuperacionCartera;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Cobranza.Modelos;
using HD_Cobranza.Capturas;
using HD_Cobranza.Reportes;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class ReporteRecuperacionCarteraController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public ReporteRecuperacionCarteraController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerReporteRecuperacionCartera(mdlFechas obj)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ReporteRecuperacionCartera_Obtener datos = new AD_ReporteRecuperacionCartera_Obtener(CadenaConexion);
            DateTime Fechainicio = DateTime.Parse(obj.fechainicio);
            DateTime Fechafinal = DateTime.Parse(obj.fechafinal);
            var result = await datos.Listado(Fechainicio, Fechafinal);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(mdlFechas obj)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ReporteRecuperacionCartera_Obtener datos = new AD_ReporteRecuperacionCartera_Obtener(CadenaConexion);
            DateTime Fechainicio = DateTime.Parse(obj.fechainicio);
            DateTime Fechafinal = DateTime.Parse(obj.fechafinal);
            var result = await datos.Listado(Fechainicio, Fechafinal);
            var docresult = await XLSCob_ReporteRecuperacionCartera_Detalle.CrearReporteRecuperacionCartera(result);
            return Ok(docresult);
        }
    }
}
