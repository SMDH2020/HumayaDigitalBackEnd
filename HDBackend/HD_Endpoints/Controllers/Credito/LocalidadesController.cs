using HD.Clientes.Consultas.Clientes;
using HD.Clientes.Consultas.Domicilios;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class LocalidadesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public LocalidadesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(string codigo_postal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Localidad_Obtener_PorCP datos = new AD_Localidad_Obtener_PorCP(CadenaConexion);
            var result = await datos.Listado(codigo_postal);
            return Ok(result);

        }
    }
}
