using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Capturas.Dashboard;
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
        public async Task<ActionResult> ReporteGraficaTotal(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaTotal(ejercicio, periodo, tipo_grafica, tipo_cartera, estado_cartera, responsable_cobranza);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteGraficaRecuperacion(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaRecuperacion(ejercicio, periodo, tipo_grafica, tipo_cartera, estado_cartera, responsable_cobranza);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteGraficaObjetivos(int ejercicio, int periodo, string tipo_grafica, string tipo_cartera, string estado_cartera, string responsable_cobranza, string categoria)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaObjetivos(ejercicio, periodo, tipo_grafica, tipo_cartera, estado_cartera, responsable_cobranza, categoria);
            return Ok(result);
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
        public async Task<ActionResult> ReporteGraficaProyeccionMensual(int ejercicio, int periodo, string mes, string sucursales, string adr, string tipo_cartera)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Reportes datos = new AD_Dashboard_Reportes(CadenaConexion);
            var result = await datos.ReporteGraficaProyeccionMensual(ejercicio, periodo, mes, sucursales, adr, tipo_cartera);
            return Ok(result);
        }
    }
}
