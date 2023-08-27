using HD.Clientes.Consultas.Clientes_Hectareas;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ClientesHectareasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesHectareasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlClientes_Hectareas mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_Clientes_Hectareas_Guardar datos = new AD_Clientes_Hectareas_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> Listado(short filtrar)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_Clientes_Hectareas_Listado datos = new AD_Clientes_Hectareas_Listado(CadenaConexion);
            var result = await datos.Listado(filtrar);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> BuscarID(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_Clientes_Hectareas_BuscarID datos = new AD_Clientes_Hectareas_BuscarID(CadenaConexion);
            var result = await datos.BuscarID(idcliente);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownList()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_Clientes_Hectareas_DropDownList datos = new AD_Clientes_Hectareas_DropDownList(CadenaConexion);
            var result = await datos.DropDownList();
            return Ok(result);

        }
    }
}
