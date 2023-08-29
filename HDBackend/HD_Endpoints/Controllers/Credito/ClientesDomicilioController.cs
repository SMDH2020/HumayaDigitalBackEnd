using HD.Clientes.Consultas.ClientesDomicilio;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ClientesDomicilioController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesDomicilioController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlClientes_Domicilio mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_ClientesDomicilio_Guardar datos = new AD_ClientesDomicilio_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> Listado(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_ClientesDomicilio_Listado datos = new AD_ClientesDomicilio_Listado(CadenaConexion);
            var result = await datos.Listado(idcliente);
            return Ok(result);

        }

    }
}
