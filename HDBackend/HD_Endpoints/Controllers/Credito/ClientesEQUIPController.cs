using HD.Clientes.Consultas.ClientesEQUIP;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ClientesEQUIPController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesEQUIPController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlClientes_EQUIP mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_ClientesEQUIP_Guardar datos = new AD_ClientesEQUIP_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> Listado(short filtrar)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_ClientesEQUIP_Listado datos = new AD_ClientesEQUIP_Listado(CadenaConexion);
            var result = await datos.Listado(filtrar);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> BuscarID(int idcliente_equip)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_ClientesEQUIP_BuscarID datos = new AD_ClientesEQUIP_BuscarID(CadenaConexion);
            var result = await datos.BuscarID(idcliente_equip);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownList()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_ClientesEQUIP_DropDownList datos = new AD_ClientesEQUIP_DropDownList(CadenaConexion);
            var result = await datos.DropDownList();
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Sucursales(string idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Sucursales_Equip datos = new AD_Clientes_Sucursales_Equip(CadenaConexion);
            var result = await datos.BuscarID(idcliente);
            return Ok(result);

        }


    }
}
