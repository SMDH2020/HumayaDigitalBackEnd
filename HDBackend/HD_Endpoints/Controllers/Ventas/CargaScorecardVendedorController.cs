using HD_Ventas.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class CargaScorecardVendedorController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CargaScorecardVendedorController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> MostrarScorecardVendedor(int ejercicio, int vendedor)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Scorecard_Vendedor datos = new AD_Carga_Scorecard_Vendedor(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Scorecard(ejercicio, usuario, vendedor);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> MostrarScorecardRealVendedor(int ejercicio, int vendedor)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Scorecard_Real_Vendedor datos = new AD_Carga_Scorecard_Real_Vendedor(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Scorecard(ejercicio, usuario, vendedor);
            return Ok(result);
        }
    }
}
