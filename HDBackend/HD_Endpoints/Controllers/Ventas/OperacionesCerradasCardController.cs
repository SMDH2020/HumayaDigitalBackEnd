using HD.Security;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos.SolicitudesCerradas;
using Microsoft.AspNetCore.Mvc;
using Ventas.Consultas;

namespace HD.Endpoints.Controllers.Ventas
{
    public class OperacionesCerradasCardController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public OperacionesCerradasCardController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }


        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener(mdlOperacionesCerradasCardsView mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Operaciones_Cerradas_Cards datos = new AD_Operaciones_Cerradas_Cards(CadenaConexion);
            var result = await datos.Obtener(mdl);
            return Ok(result);
        }
    }
}
