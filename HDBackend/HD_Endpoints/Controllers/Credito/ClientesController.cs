using HD.Clientes.Consultas.Clientes;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ClientesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Persona_Fisica(mdlClientes_Datos_Persona_Fisica mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Guardar datos = new AD_Clientes_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            mdl.idcliente = await datos.Guardar_Persona_Fisica(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Persona_Moral(mdlClientes mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Guardar datos = new AD_Clientes_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            mdl.idcliente = await datos.Guardar_Persona_Moral(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Persona_Moral_Vendedor(mdlClientes mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Guardar datos = new AD_Clientes_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            mdl.idcliente = await datos.Guardar_Persona_Moral(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> Listado(short filtrar)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Listado datos = new AD_Clientes_Listado(CadenaConexion);
            var result = await datos.Listado(filtrar);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarID(string idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_BuscarID datos = new AD_Clientes_BuscarID(CadenaConexion);
            var result = await datos.BuscarID(int.Parse(idcliente));
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownList()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_DropDownList datos = new AD_Clientes_DropDownList(CadenaConexion);
            var result = await datos.DropDownList();
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarRFCOrRazonSocial(string value)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_BuscarRFCOrRazonSocial datos = new AD_Clientes_BuscarRFCOrRazonSocial(CadenaConexion);
            var result = await datos.Listado(value);
            return Ok(result);

        }
    }
}
