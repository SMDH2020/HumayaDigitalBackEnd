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

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ClientesDomicilio_Guardar datos = new AD_ClientesDomicilio_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result =await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito", domicilios = result });

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerporOrden(int idcliente,int orden)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ClientesDomicilio_ObtenerPorOrden datos = new AD_ClientesDomicilio_ObtenerPorOrden(CadenaConexion);
            var result = await datos.Get(idcliente,orden);
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ClientesDomicilio_Listado datos = new AD_ClientesDomicilio_Listado(CadenaConexion);
            var result = await datos.Listado(idcliente);
            return Ok(result);

        }

    }
}
