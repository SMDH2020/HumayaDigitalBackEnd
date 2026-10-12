using HD.Security;
using HD_Ventas.DashboardRefacciones;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.Dashboard;

namespace HD.Endpoints.Controllers.DashboardRefacciones
{
    public class DashboardRefaccionesController :MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public DashboardRefaccionesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Dashboard(string? fechainicio, string? fechafin, string? vendedor, string? cliente, string comparativa, string? adr, string? sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Refacciones_Detalle datos = new AD_Dashboard_Refacciones_Detalle(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerDashboard( fechainicio, fechafin, vendedor,cliente, comparativa, adr, sucursal);
            return Ok(result);
        }
    }
}
