using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Reportes;
using HD_Reporteria.Cobranza;
using Microsoft.AspNetCore.Mvc;
using HD_Reporteria;
using System.Globalization;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class RecuperacionCarteraMensualController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public RecuperacionCarteraMensualController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener(int ejercicio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADRecuperacionCarteraMensual datos = new ADRecuperacionCarteraMensual(CadenaConexion);
            var result = await datos.Obtener(ejercicio);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerObjetivoRecuperado(int ejercicio,string sucursales,string adr)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADRecuperacionCarteraMensual datos = new ADRecuperacionCarteraMensual(CadenaConexion);
            var result = await datos.ObtenerObjetivoRecuperado(ejercicio,adr,sucursales);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcel(int ejercicio, string sucursales, string adr)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADRecuperacionCarteraMensual datos = new ADRecuperacionCarteraMensual(CadenaConexion);
            var result = await datos.ObtenerObjetivoRecuperado(ejercicio, adr, sucursales);
            var docresult = await XLSCob_Reporte_Recuperacion_Cartera_Mensual.GenerarExcel(result);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDF(int ejercicio, string sucursales, string adr)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADRecuperacionCarteraMensual datos = new ADRecuperacionCarteraMensual(CadenaConexion);
            var result = await datos.ObtenerObjetivoRecuperado(ejercicio, adr, sucursales);

            try
            {
                RPT_Result documento = RPT_Recuperacion_Cartera_Mensual.GenerarPDF(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
