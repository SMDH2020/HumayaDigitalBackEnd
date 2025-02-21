using HD.Clientes.Consultas.AnalisisCredito;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito
{
    public class ACTimeLineController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACTimeLineController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_TimeLine datos = new ADAnalisis_TimeLine(CadenaConexion);
            var result = await datos.BuscarFolio(folio,Sesion.usuario());
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoCondicionado(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Timeline_Condicionado datos = new ADAnalisis_Timeline_Condicionado(CadenaConexion);
            var result = await datos.BuscarFolio(folio, Sesion.usuario());
            return Ok(result);

        }
    }
}
