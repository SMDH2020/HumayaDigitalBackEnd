using HD.Clientes.Consultas.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ModelosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ModelosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(string linea)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADModelos datos = new ADModelos(CadenaConexion);
            var result = await datos.GetModelos(linea);
            return Ok(result);

        }
    }
}
