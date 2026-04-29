using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Capturas.Dashboard;
using HD_Cobranza.Capturas.Dashboard.Dash_Indicadores;
using HD_Cobranza.Reportes;
using HD_Reporteria;
using HD_Reporteria.Cobranza;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza.Dashboard.Dashboard_Indicadores
{
    public class DashboardIndicadoresGraficasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public DashboardIndicadoresGraficasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerInfoGraficas(int ejercicio, int periodo, string adr, string sucursales, string responsable)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Indicadores_Graficas datos = new AD_Dashboard_Indicadores_Graficas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerGraficas(ejercicio, periodo, adr, sucursales, responsable, usuario);
            return Ok(result);
        }
    }
}
