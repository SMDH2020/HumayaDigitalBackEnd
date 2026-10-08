using HD.Security;
using HD_CentroMonitoreo.Consultas.Dashboard;
using HD_CentroMonitoreo.Modelos.Dashboard;
using HD_Cobranza.Capturas.ConvenioPago;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.CentroMonitoreo
{
    public class CentroMonitoreoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CentroMonitoreoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        /// <summary>
        /// Dashboard de Soluciones Integrales: datos del rango consultado y del periodo anterior.
        /// El usuario sale de la sesion; el SP restringe a las sucursales que tiene permitidas.
        /// </summary>
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Dashboard([FromQuery] mdl_Dashboard_Filtro filtro)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard_Obtener datos = new AD_Dashboard_Obtener(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Obtener(filtro, usuario);
            return Ok(result);
        }
    }
}
