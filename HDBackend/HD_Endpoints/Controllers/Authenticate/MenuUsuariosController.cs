using HD.Generales.Autenticate;
using HD.Generales.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Authenticate
{
    public class MenuUsuariosController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public MenuUsuariosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Post(mdlUsuarioMenu mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RelUsuarioMenu_Guardar datos = new AD_RelUsuarioMenu_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoModulos()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Modulos_Listado datos = new AD_Modulos_Listado(CadenaConexion);
            var result = await datos.ListadoModulos();
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoUsuario(int idusuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_UsuarioMenu_Listado datos = new AD_UsuarioMenu_Listado(CadenaConexion);
            var result = await datos.ListadoUsuario(idusuario);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoMenu(int idmodulo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RelMenuModulo_Listado datos = new AD_RelMenuModulo_Listado(CadenaConexion);
            var result = await datos.ListadoMenu(idmodulo);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoUsuarioMenu(int idusuario, int idmodulo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RelUsuariosMenu_Listado datos = new AD_RelUsuariosMenu_Listado(CadenaConexion);
            var result = await datos.ListadoUsuarioMenu(idusuario, idmodulo);
            return Ok(result);

        }
    }
}
