using HD.Clientes.Consultas.CRM.IndicadoresVisitas;
using HD.Security;
using HD_Reporteria.CRM;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class IndicadoresVisitasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public IndicadoresVisitasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteVisitas(int ejercicio, int periodo, string? linea, string? tipo = null)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_IndicadoresVisitas_ReporteVisitas datos = new AD_IndicadoresVisitas_ReporteVisitas(CadenaConexion);
            var result = await datos.ReporteVisitas(ejercicio, periodo, TipoConsulta(linea, tipo));
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteVisitas(int ejercicio, int periodo, string? linea, string? tipo = null)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_IndicadoresVisitas_ReporteVisitas datos = new AD_IndicadoresVisitas_ReporteVisitas(CadenaConexion);
            string? tipoConsulta = TipoConsulta(linea, tipo);
            var result = await datos.ReporteVisitas(ejercicio, periodo, tipoConsulta);
            var docresult = await XLS_Reporte_IndicadoresVisitas.GenerarExcel(result, ejercicio, periodo, tipoConsulta);
            return Ok(docresult);
        }

        /// <summary>
        /// El front manda el tipo de visita (P = Programadas, R = Realizadas) en el
        /// parametro linea; se acepta tipo como alias para no romper llamadas previas.
        /// </summary>
        private static string? TipoConsulta(string? linea, string? tipo)
        {
            return string.IsNullOrWhiteSpace(linea) ? tipo : linea;
        }
    }
}
