using HD.Clientes.Consultas.Clientes_Juridico;
using HD.Clientes.Modelos.Clientes_Juridico;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HD.Endpoints.Controllers.Credito.ClientesJuridico
{
    public class ClientesJuridicoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesJuridicoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ClientesJuridico()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Clientes_Juridico datos = new AD_Listado_Clientes_Juridico(CadenaConexion);
            var result = await datos.clientes();
            return Ok(result);
        }
    }
}
