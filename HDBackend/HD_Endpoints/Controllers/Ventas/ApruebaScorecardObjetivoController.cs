using HD_Ventas.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Ventas.Modelos;

namespace HD.Endpoints.Controllers.Ventas
{
    public class ApruebaScorecardObjetivoController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ApruebaScorecardObjetivoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Aprueba(mdlAprueba_Scorecard_Objetivo mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Aprueba_Scorecard_Objetivo datos = new AD_Aprueba_Scorecard_Objetivo(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Aprueba(mdl);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Modifica(mdlModifica_Scorecard_Objetivo mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Modifica_Scorecard_Objetivo datos = new AD_Modifica_Scorecard_Objetivo(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Modifica(mdl);
            return Ok(result);
        }
    }
}
