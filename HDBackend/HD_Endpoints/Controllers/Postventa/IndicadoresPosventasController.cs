using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.Dashboard;
using Postventa.Consultas.Indicadores;

namespace HD.Endpoints.Controllers.Postventa
{
    public class IndicadoresPosventasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public IndicadoresPosventasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Indicadores(int ejercicio_inicio, int ejercicio_fin, int periodo_inicio, int periodo_fin, string seccion, string sucursales, string adr)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Indicadores_Posventa datos = new AD_Indicadores_Posventa(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerIndicadores(ejercicio_inicio, ejercicio_fin, periodo_inicio, periodo_fin, seccion, sucursales, adr);
            return Ok(result);
        }
    }
}
