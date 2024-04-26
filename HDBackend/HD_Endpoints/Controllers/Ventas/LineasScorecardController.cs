using HD.Generales.Autenticate;
using HD.Generales.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Ventas.Consultas;
using Ventas.Modelos;

namespace HD.Endpoints.Controllers.Ventas
{
    public class LineasScorecardController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public LineasScorecardController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Post(mdl_LineasScorecard mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_LineasScorecard_Guardar datos = new AD_LineasScorecard_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_LineasScorecard_Listado datos = new AD_LineasScorecard_Listado(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);

        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarID(int idlinea)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_LineasScorecard_BuscarID datos = new AD_LineasScorecard_BuscarID(CadenaConexion);
            var result = await datos.BuscarID(idlinea);
            return Ok(result);

        }
    }
}
