using HD.Clientes.Consultas.ClientesCultivo;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ClientesCultivoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesCultivoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlClientes_Cultivo mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ClientesCultivo_Guardar datos = new AD_ClientesCultivo_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerporOrden(int idcliente, int registro)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ClientesCultivo_OptenerPorOrden datos = new AD_ClientesCultivo_OptenerPorOrden(CadenaConexion);
            var result = await datos.Get(idcliente, registro);
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ClientesCultivo_Listado datos = new AD_ClientesCultivo_Listado(CadenaConexion);
            var result = await datos.Listado(idcliente);
            return Ok(result);

        }
    }
}
