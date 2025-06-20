using HD.Generales.Consultas;
using HD.Security;
using HD_Helpers.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Generales
{
    public class PresasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PresasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ComparativaPresas()
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Comparativa_Presas_Listado datos = new AD_Comparativa_Presas_Listado(CadenaConexion);
            var result = await datos.listadoPresas();
            return Ok(result);

        }
    }
}
