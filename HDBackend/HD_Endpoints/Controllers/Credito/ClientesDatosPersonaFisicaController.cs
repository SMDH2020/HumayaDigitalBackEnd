using HD.Clientes.Consultas.ClientesDatosPersonaFisica;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ClientesDatosPersonaFisicaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesDatosPersonaFisicaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlClientes_Datos_Persona_Fisica mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_ClientesDatosPersonaFisica_Guardar datos = new AD_ClientesDatosPersonaFisica_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> Listado(short filtrar)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_ClientesDatosPersonaFisica_Listado datos = new AD_ClientesDatosPersonaFisica_Listado(CadenaConexion);
            var result = await datos.Listado(filtrar);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> BuscarID(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_ClientesDatosPersonaFisica_BuscarID datos = new AD_ClientesDatosPersonaFisica_BuscarID(CadenaConexion);
            var result = await datos.BuscarID(idcliente);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownList()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_ClientesDatosPersonaFisica_DropDownList datos = new AD_ClientesDatosPersonaFisica_DropDownList(CadenaConexion);
            var result = await datos.DropDownList();
            return Ok(result);

        }
    }
}
