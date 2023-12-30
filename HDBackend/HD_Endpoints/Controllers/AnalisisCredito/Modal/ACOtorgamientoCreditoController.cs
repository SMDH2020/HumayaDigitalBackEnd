using HD.Clientes.Consultas.AnalisisCredito.Modal;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.Modal
{
    public class ACOtorgamientoCreditoController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACOtorgamientoCreditoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Get(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Otorgamiento_Credito datos = new ADAnalisis_Otorgamiento_Credito(CadenaConexion);
            var result = await datos.Get(folio, Sesion.usuario());
            return Ok(result);

        }
    }
}
