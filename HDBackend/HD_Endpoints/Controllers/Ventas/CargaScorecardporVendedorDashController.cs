using HD.Notifications.NotificacionesApp;
using HD.Security;
using HD_Reporteria;
using HD_Reporteria.Ventas;
using HD_Ventas.Consultas;
using HD_Ventas.Reportes;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class CargaScorecardporVendedorDashController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CargaScorecardporVendedorDashController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> MostrarScorecardVendedor()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Scorecard_porVendedor_Dash datos = new AD_Carga_Scorecard_porVendedor_Dash(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            //usuario = 5630;
            var result = await datos.Scorecard(usuario);

            string origen = Sesion.origen();
            if (Sesion.generarLog() == true)
            {
                NE_Logs_App_HD log = new NE_Logs_App_HD(CadenaConexion);
                await log.Guardar("Navego a Scorecard", origen, Sesion.usuario());
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> MostrarScorecardVendedorporID(int vendedor)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Scorecard_porVendedor_Dash datos = new AD_Carga_Scorecard_porVendedor_Dash(CadenaConexion);
            int usuario = vendedor;
            var result = await datos.Scorecard(usuario);
            return Ok(result);
        }

        [HttpGet] 
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> MostrarScorecardVendedorporParametros(int region, string sucursal, string vendedor, int ejercicioinicio, int periodoinicio, int ejercicio, int mes_actual)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Scorecard_porParametros_Dash datos = new AD_Carga_Scorecard_porParametros_Dash(CadenaConexion);
            string usuario = vendedor;
            int sesion = int.Parse(Sesion.usuario());
            //sesion = 5630;
            var result = await datos.Scorecard(region, sucursal, usuario, ejercicioinicio, periodoinicio, ejercicio, mes_actual, sesion);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> MostrarScorecardVendedorporParametrosTablaAsesor(int region, string? sucursal, string? vendedor, int ejercicioinicio, int periodoinicio, int ejercicio, int mes_actual)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Scorecard_porParametros_Dash datos = new AD_Carga_Scorecard_porParametros_Dash(CadenaConexion);
            string? usuario = vendedor;
            int sesion = int.Parse(Sesion.usuario());
            //sesion = 5630;
            var result = await datos.Scorecard_TablaAsesor(region, sucursal, usuario, ejercicioinicio, periodoinicio, ejercicio, mes_actual, sesion);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> MostrarScorecardVendedorporParametrosTablaAsesorImportes(int region, string sucursal, string vendedor, int ejercicioinicio, int periodoinicio, int ejercicio, int mes_actual)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Scorecard_porParametros_Dash datos = new AD_Carga_Scorecard_porParametros_Dash(CadenaConexion);
            string usuario = vendedor;
            int sesion = int.Parse(Sesion.usuario());
            //sesion = 5630;
            var result = await datos.Scorecard_TablaAsesor_importes(region, sucursal, usuario, ejercicioinicio, periodoinicio, ejercicio, mes_actual, sesion);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcel(int region, string sucursal, string vendedor, int ejercicioinicio, int periodoinicio, int ejercicio, int mes_actual)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Scorecard_porParametros_Dash datos = new AD_Carga_Scorecard_porParametros_Dash(CadenaConexion);
            string usuario = vendedor;
            int sesion = int.Parse(Sesion.usuario());
            var result = await datos.Scorecard(region, sucursal, usuario, ejercicioinicio, periodoinicio, ejercicio, mes_actual, sesion);
            var docresult = await XLSVen_Scorecard_General_Dash.GenerarExcel(result, ejercicio, mes_actual, ejercicioinicio, periodoinicio);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelTablaAsesores(int region, string? sucursal, string? vendedor, int ejercicioinicio, int periodoinicio, int ejercicio, int mes_actual)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Scorecard_porParametros_Dash datos = new AD_Carga_Scorecard_porParametros_Dash(CadenaConexion);
            string? usuario = vendedor;
            int sesion = int.Parse(Sesion.usuario());
            var result = await datos.Scorecard_TablaAsesor(region, sucursal, usuario, ejercicioinicio, periodoinicio, ejercicio, mes_actual, sesion);
            var docresult = await XLSVen_Scorecard_Asesores_Table.GenerarExcel(result, ejercicio, mes_actual, ejercicioinicio, periodoinicio);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDF(int region, string sucursal, string vendedor, int ejercicioinicio, int periodoinicio, int ejercicio, int mes_actual)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Scorecard_porParametros_Dash datos = new AD_Carga_Scorecard_porParametros_Dash(CadenaConexion);
            string usuario = vendedor;
            int sesion = int.Parse(Sesion.usuario());
            var result = await datos.Scorecard(region, sucursal, usuario, ejercicioinicio, periodoinicio, ejercicio, mes_actual, sesion);

            try
            {
                RPT_Result documento = RPT_Scorecard_General_Dash.GenerarPDF(result, ejercicio, mes_actual, ejercicioinicio, periodoinicio);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDFTablaAsesores(int region, string? sucursal, string? vendedor, int ejercicioinicio, int periodoinicio, int ejercicio, int mes_actual)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Scorecard_porParametros_Dash datos = new AD_Carga_Scorecard_porParametros_Dash(CadenaConexion);
            string? usuario = vendedor;
            int sesion = int.Parse(Sesion.usuario());
            var result = await datos.Scorecard_TablaAsesor(region, sucursal, usuario, ejercicioinicio, periodoinicio, ejercicio, mes_actual, sesion);

            try
            {
                RPT_Result documento = RPT_Scorecard_Asesores_Tabla.GenerarPDF(result, ejercicio, mes_actual, ejercicioinicio, periodoinicio);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
