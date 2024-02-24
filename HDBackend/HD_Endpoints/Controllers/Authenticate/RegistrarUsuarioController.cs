using HD.Clientes.Consultas.Clientes;
using HD.Clientes.Consultas.GiroEmpresarial;
using HD.Clientes.Modelos;
using HD.Generales.Autenticate;
using HD.Generales.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Authenticate
{
    public class RegistrarUsuarioController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public RegistrarUsuarioController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Post(mdlUsuarios mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_Usuarios_Guardar datos = new AD_Usuarios_Guardar(CadenaConexion);
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownList()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Usuarios_Nuevos_Dropdownlist datos = new AD_Usuarios_Nuevos_Dropdownlist(CadenaConexion);
            var result = await datos.DropDownList();
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Buscar(int idusuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Usuarios_Search datos = new AD_Usuarios_Search(CadenaConexion);
            var result = await datos.Listado(idusuario);
            return Ok(result);

        }
    }
}
