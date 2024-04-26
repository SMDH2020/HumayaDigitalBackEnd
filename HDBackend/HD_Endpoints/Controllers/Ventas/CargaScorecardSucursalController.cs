using HD_Ventas.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class CargaScorecardSucursalController : ControllerBase // Heredar de ControllerBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public CargaScorecardSucursalController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> MostrarScorecardSucursal(int ejercicio, int sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Scorecard_Sucursal datos = new AD_Carga_Scorecard_Sucursal(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            usuario = 8919;
            var result = await datos.Scorecard(ejercicio, sucursal);
            return Ok(result);
        }
    }
}