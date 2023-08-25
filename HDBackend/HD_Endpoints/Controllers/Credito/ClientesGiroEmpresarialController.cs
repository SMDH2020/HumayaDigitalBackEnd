using HD.Clientes.Consultas.ClientesGiroEmpresarial;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ClientesGiroEmpresarialController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesGiroEmpresarialController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlClientes_Giro_Empresarial mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_ClientesGiroEmpresarial_Guardar datos = new AD_ClientesGiroEmpresarial_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> Listado(short filtrar)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_ClientesGiroEmpresarial_Listado datos = new AD_ClientesGiroEmpresarial_Listado(CadenaConexion);
            var result = await datos.Listado(filtrar);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> BuscarID(int idcliente_giro_empresarial)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_ClientesGiroEmpresarial_BuscarID datos = new AD_ClientesGiroEmpresarial_BuscarID(CadenaConexion);
            var result = await datos.BuscarID(idcliente_giro_empresarial);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownList()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_ClientesGiroEmpresarial_DropDownList datos = new AD_ClientesGiroEmpresarial_DropDownList(CadenaConexion);
            var result = await datos.DropDownList();
            return Ok(result);

        }
    }
}
