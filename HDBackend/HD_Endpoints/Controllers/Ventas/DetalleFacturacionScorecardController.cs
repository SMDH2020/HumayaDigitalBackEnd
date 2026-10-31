using HD.Security;
using HD_Reporteria.Ventas;
using HD_Ventas.Consultas;
using HD_Ventas.Reportes;
using Microsoft.AspNetCore.Mvc;
using HD_Reporteria;

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
        public async Task<ActionResult> ImprimirExcelMensual(string linea, int ejercicio, int periodo, string adr, string sucursal, string vendedor)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Facturacion_Scorecard_Mensual datos = new AD_Detalle_Facturacion_Scorecard_Mensual(CadenaConexion);
            var result = await datos.Get(linea, ejercicio, periodo, adr, sucursal, vendedor);
            var docresult = await XLSVen_Detalle_Facturacion_Scorecard_Mensual.GenerarExcel(result, ejercicio, periodo);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDFMensual(string linea, int ejercicio, int periodo, string adr, string sucursal, string vendedor)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Facturacion_Scorecard_Mensual datos = new AD_Detalle_Facturacion_Scorecard_Mensual(CadenaConexion);
            var result = await datos.Get(linea, ejercicio, periodo, adr, sucursal, vendedor);

            try
            {
                RPT_Result documento = RPT_Detalle_Facturacion_Scorecard_Mensual.GenerarPDF(result, ejercicio, periodo);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

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

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelAcumulado(string linea, int ejercicioinicio, int periodoinicio, int ejerciciofin, int periodofin, string adr, string sucursal, string vendedor)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Facturacion_Scorecard_Acumulado datos = new AD_Detalle_Facturacion_Scorecard_Acumulado(CadenaConexion);
            var result = await datos.Get(linea, ejercicioinicio, periodoinicio, ejerciciofin, periodofin, adr, sucursal, vendedor);
            var docresult = await XLSVen_Detalle_Facturacion_Scorecard_Acumulado.GenerarExcel(result, ejercicioinicio, periodoinicio, ejerciciofin, periodofin);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDFAcumulado(string linea, int ejercicioinicio, int periodoinicio, int ejerciciofin, int periodofin, string adr, string sucursal, string vendedor)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Facturacion_Scorecard_Acumulado datos = new AD_Detalle_Facturacion_Scorecard_Acumulado(CadenaConexion);
            var result = await datos.Get(linea, ejercicioinicio, periodoinicio, ejerciciofin, periodofin, adr, sucursal, vendedor);

            try
            {
                RPT_Result documento = RPT_Detalle_Facturacion_Scorecard_Acumulado.GenerarPDF(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
