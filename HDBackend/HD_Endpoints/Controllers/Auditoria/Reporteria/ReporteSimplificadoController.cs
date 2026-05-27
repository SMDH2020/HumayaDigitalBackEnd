using HD.Security;
using HD_Auditoria.Consultas.Conteo_Piezas;
using HD_Auditoria.Consultas.Reporteria;
using HD_Auditoria.Reporteria;
using HD_Cobranza.Reportes;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Auditoria.Reporteria
{
    public class ReporteSimplificadoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ReporteSimplificadoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Simplificado datos = new AD_Reporte_Simplificado(CadenaConexion);
            var result = await datos.ReporteSimplificado(folio);
            var docresult = await XLS_Reporte_Simplificado.GenerarExcel(result, folio);
            return Ok(docresult);
        }
    }
}
