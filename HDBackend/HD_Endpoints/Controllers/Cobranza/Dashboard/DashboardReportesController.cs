using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Capturas.Dashboard;
using HD_Cobranza.Capturas.ReporteRecuperacionCompleta;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.Reportes;
using HD_Reporteria;
using HD_Reporteria.Cobranza;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza.Dashboard
{
    public class DashboardReportesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public DashboardReportesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteGraficaTotal(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza, string adr, string sucursales)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaTotal(ejercicio, periodo, tipo_grafica, tipo_cartera, estado_cartera, responsable_cobranza, adr, sucursales);
            return Ok(result);
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteTotal(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza, string adr, string sucursales)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaTotal(ejercicio, periodo, tipo_grafica, tipo_cartera, estado_cartera, responsable_cobranza, adr, sucursales);
            var docresult = await XLSCob_Dashboard_ReporteTotal.GenerarExcel(result, ejercicio, periodo, tipo_cartera, tipo_grafica, estado_cartera, responsable_cobranza);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDFReporteTotal(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza, string adr, string sucursales)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaTotal(ejercicio, periodo, tipo_grafica, tipo_cartera, estado_cartera, responsable_cobranza, adr, sucursales);

            try
            {
                RPT_Result documento = RPT_Dashboard_ReporteTotal.GenerarPDF(result, ejercicio, periodo, tipo_cartera,tipo_grafica, estado_cartera, responsable_cobranza);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteGraficaRecuperacion(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza, string adr, string sucursales)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaRecuperacion(ejercicio, periodo, tipo_grafica, tipo_cartera, estado_cartera, responsable_cobranza, adr, sucursales);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteRecuperacion(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza, string adr, string sucursales)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaRecuperacion(ejercicio, periodo, tipo_grafica, tipo_cartera, estado_cartera, responsable_cobranza, adr, sucursales);
            var docresult = await XLSCob_Dashboard_ReporteRecuperacion.GenerarExcel(result, ejercicio, periodo, tipo_cartera, tipo_grafica, estado_cartera, responsable_cobranza);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDFReporteRecuperacion(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza, string adr, string sucursales)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaRecuperacion(ejercicio, periodo, tipo_grafica, tipo_cartera, estado_cartera, responsable_cobranza, adr, sucursales);

            try
            {
                RPT_Result documento = RPT_Dashboard_ReporteRecuperacion.GenerarPDF(result, ejercicio, periodo, tipo_cartera, tipo_grafica, estado_cartera, responsable_cobranza);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteGraficaObjetivos(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza, string categoria, string adr, string sucursales)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaObjetivos(ejercicio, periodo, tipo_grafica, tipo_cartera, estado_cartera, responsable_cobranza, categoria, adr, sucursales);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteObjetivo(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza, string categoria, string adr, string sucursales)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaObjetivos(ejercicio, periodo, tipo_grafica, tipo_cartera, estado_cartera, responsable_cobranza, categoria, adr, sucursales);
            var docresult = await XLSCob_Dashboard_ReporteObjetivo.GenerarExcel(result, ejercicio, periodo, tipo_cartera, tipo_grafica, estado_cartera, responsable_cobranza, categoria);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDFReporteObjetivo(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza, string categoria, string adr, string sucursales)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaObjetivos(ejercicio, periodo, tipo_grafica, tipo_cartera, estado_cartera, responsable_cobranza, categoria, adr, sucursales);

            try
            {
                RPT_Result documento = RPT_Dashboard_ReporteObjetivo.GenerarPDF(result, ejercicio, periodo, tipo_cartera, tipo_grafica, estado_cartera, responsable_cobranza, categoria);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteGraficaProyeccionTotal(int ejercicio, int periodo, string mes, string sucursales, string adr)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaProyeccionTotal(ejercicio, periodo, mes, sucursales, adr);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteProyeccionTotal(int ejercicio, int periodo, string mes, string sucursales, string adr)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaProyeccionTotal(ejercicio, periodo, mes, sucursales, adr);
            var docresult = await XLSCob_Dashboard_ReporteProyeccionTotal.GenerarExcel(result, ejercicio, periodo, mes, sucursales, adr);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDFReporteProyeccionTotal(int ejercicio, int periodo, string mes, string sucursales, string adr)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaProyeccionTotal(ejercicio, periodo, mes, sucursales, adr);

            try
            {
                RPT_Result documento = RPT_Dashboard_ReporteProyeccionTotal.GenerarPDF(result, ejercicio, periodo, mes, sucursales, adr);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteGraficaProyeccionMensual(int ejercicio, int periodo, string mes, string sucursales, string adr, string tipo_cartera)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaProyeccionMensual(ejercicio, periodo, mes, sucursales, adr, tipo_cartera);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteProyeccionMensual(int ejercicio, int periodo, string mes, string sucursales, string adr, string tipo_cartera)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaProyeccionMensual(ejercicio, periodo, mes, sucursales, adr, tipo_cartera);
            var docresult = await XLSCob_Dashboard_ReporteProyeccionMensual.GenerarExcel(result, ejercicio, periodo, mes, sucursales, adr, tipo_cartera);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDFReporteProyeccionMensual(int ejercicio, int periodo, string mes, string sucursales, string adr, string tipo_cartera)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaProyeccionMensual(ejercicio, periodo, mes, sucursales, adr, tipo_cartera);

            try
            {
                RPT_Result documento = RPT_Dashboard_ReporteProyeccionMensual.GenerarPDF(result, ejercicio, periodo, mes, sucursales, adr, tipo_cartera);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
