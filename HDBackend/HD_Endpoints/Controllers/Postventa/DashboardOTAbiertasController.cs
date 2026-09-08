using HD.Security;
using HD_Ventas.Consultas;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.OrdenesAbiertas;

namespace HD.Endpoints.Controllers.Ventas
{
    public class DashboardOTAbiertasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public DashboardOTAbiertasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerOrdenesAbiertas()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_DashboardOTAbiertas datos = new AD_DashboardOTAbiertas(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            
            var result = await datos.ObtenerOrdenesAbiertas(usuario);
            return Ok(result);
        }
    }
}