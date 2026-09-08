using HD.Clientes.Consultas.Credito_Condicionado;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.Modal
{
    public class ACDValidarCondicionesFinanzasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACDValidarCondicionesFinanzasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Get(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Credito_Condicionado_Finanzas datos = new AD_Credito_Condicionado_Finanzas(CadenaConexion);
            var result = await datos.Obtener(folio, Sesion.usuario());
            return Ok(result);

        }
    }
}
