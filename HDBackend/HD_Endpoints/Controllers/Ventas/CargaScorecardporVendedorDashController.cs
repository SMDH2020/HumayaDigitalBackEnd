using HD_Ventas.Consultas;
using HD.Security;
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

        public async Task<ActionResult> MostrarScorecardVendedorporParametros(int region, int sucursal, int vendedor, int ejercicioinicio, int periodoinicio, int ejercicio, int mes_actual)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Scorecard_porParametros_Dash datos = new AD_Carga_Scorecard_porParametros_Dash(CadenaConexion);
            int usuario = vendedor;
            int sesion = int.Parse(Sesion.usuario());
            //sesion = 5630;
            var result = await datos.Scorecard(region, sucursal, usuario, ejercicioinicio, periodoinicio, ejercicio, mes_actual, sesion);
            return Ok(result);
        }
    }
}
