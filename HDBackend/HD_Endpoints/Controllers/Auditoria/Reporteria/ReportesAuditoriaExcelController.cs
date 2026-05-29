using HD.Security;
using HD_Auditoria.Consultas.Reporteria;
using HD_Auditoria.Reporteria;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Auditoria.Reporteria
{
    public class ReportesAuditoriaExcelController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ReportesAuditoriaExcelController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcelSimplificado(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Simplificado datos = new AD_Reporte_Simplificado(CadenaConexion);
            var result = await datos.ReporteSimplificado(folio);
            var docresult = await XLS_Reporte_Simplificado.GenerarExcel(result, folio);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcelPrimerConteo(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Primer_Conteo datos = new AD_Reporte_Primer_Conteo(CadenaConexion);
            var result = await datos.ReportePrimerConteo(folio);
            var docresult = await XLS_Reporte_Primer_Conteo.GenerarExcel(result, folio);
            return Ok(docresult);
            return null;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcelSegundoConteo(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Segundo_Conteo datos = new AD_Reporte_Segundo_Conteo(CadenaConexion);
            var result = await datos.ReporteSegundoConteo(folio);
            var docresult = await XLS_Reporte_Segundo_Conteo.GenerarExcel(result, folio);
            return Ok(docresult);
        }
    }
}
