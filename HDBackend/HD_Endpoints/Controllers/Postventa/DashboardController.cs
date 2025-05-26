using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.GestionCobranza.Modelos;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.Dashboard;

namespace HD.Endpoints.Controllers.Postventa
{
    public class DashboardPostventaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public DashboardPostventaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Dashboard(int ejercicio, int periodo_inicio, int periodo_fin, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Postventa_Info datos = new AD_Dashboard_Postventa_Info(CadenaConexion);
            var result = await datos.ObtenerDashboard(ejercicio, periodo_inicio, periodo_fin, adr, sucursal);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> VencimientosGarantias(int ejercicio, int periodo_inicio, int periodo_fin, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Vencimiento_Garantias datos = new AD_Obtener_Vencimiento_Garantias(CadenaConexion);
            var result = await datos.ObtenerVencimientos(ejercicio, periodo_inicio, periodo_fin, adr, sucursal);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CotizacionesAbiertas(int ejercicio, int periodo_inicio, int periodo_fin, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Cotizaciones_Abiertas datos = new AD_Obtener_Cotizaciones_Abiertas(CadenaConexion);
            var result = await datos.ObtenerCotizaciones(ejercicio, periodo_inicio, periodo_fin, adr, sucursal);
            return Ok(result);
        }
    }
}
