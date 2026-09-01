using HD.Clientes.Consultas.CRM.IndicadoresCotizaciones;
using HD.Security;
using HD_Reporteria.CRM;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.CRM
{
    public class IndicadoresCotizacionesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public IndicadoresCotizacionesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteCotizaciones(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_IndicadoresCotizaciones_ReporteCotizaciones datos = new AD_IndicadoresCotizaciones_ReporteCotizaciones(CadenaConexion);
            var result = await datos.ReporteCotizaciones(ejercicio, periodo);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteCotizaciones(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_IndicadoresCotizaciones_ReporteCotizaciones datos = new AD_IndicadoresCotizaciones_ReporteCotizaciones(CadenaConexion);
            var result = await datos.ReporteCotizaciones(ejercicio, periodo);
            var docresult = await XLS_Reporte_IndicadoresCotizaciones.GenerarExcel(result, ejercicio, periodo);
            return Ok(docresult);
        }
    }
}
