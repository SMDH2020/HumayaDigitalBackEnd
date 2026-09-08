using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Capturas.ReporteRecuperacionCompleta;
using HD_Cobranza.Reportes;
using HD_Reporteria;
using HD_Reporteria.Cobranza;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza.ReporteRecuperacionCarteraCompleta
{
    public class ReporteRecuperacionCompletaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public ReporteRecuperacionCompletaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerRecuperacion(int ejercicio, string sucursales, string adr)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Recuperacion_Completa datos = new AD_Reporte_Recuperacion_Completa(CadenaConexion);
            var result = await datos.ObtenerRecuperacion(ejercicio, adr, sucursales);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcel(int ejercicio, string sucursales, string adr)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Recuperacion_Completa datos = new AD_Reporte_Recuperacion_Completa(CadenaConexion);
            var result = await datos.ObtenerRecuperacion(ejercicio, adr, sucursales);
            var docresult = await XLSCob_Reporte_Recuperacion_Completa.GenerarExcel(result);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDF(int ejercicio, string sucursales, string adr)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Recuperacion_Completa datos = new AD_Reporte_Recuperacion_Completa(CadenaConexion);
            var result = await datos.ObtenerRecuperacion(ejercicio, adr, sucursales);

            try
            {
                RPT_Result documento = RPT_Recuperacion_Completa.GenerarPDF(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
