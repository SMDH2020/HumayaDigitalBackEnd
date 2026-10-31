using HD.Security;
using HD_GestionProyectosTI.Consultas.Catalogos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionProyectosTI
{
    public class CatalogosTIController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public CatalogosTIController(IConfiguration configuracion, ISesion sesion)
        {
            Configuracion = configuracion;
            Sesion = sesion;
        }

        private string CadenaConexion => Configuracion["ConnectionStrings:GestionProyectosTI"]!;

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> TiposSolicitud()
        {
            AD_TI_Catalogos_TiposSolicitud_Listado datos = new AD_TI_Catalogos_TiposSolicitud_Listado(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CriteriosPrioridad()
        {
            AD_TI_Catalogos_CriteriosPrioridad_Listado datos = new AD_TI_Catalogos_CriteriosPrioridad_Listado(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Prioridades()
        {
            AD_TI_Catalogos_Prioridades_Listado datos = new AD_TI_Catalogos_Prioridades_Listado(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }
    }
}
