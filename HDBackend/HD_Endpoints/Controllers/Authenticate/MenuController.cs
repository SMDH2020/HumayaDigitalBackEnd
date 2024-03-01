using HD.Generales.Autenticate;
using HD.Generales.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Authenticate
{
    public class MenuController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public MenuController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Post(mdlMenu mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Menus_Guardar datos = new AD_Menus_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoMenus(int idmodulo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Menus_Listado datos = new AD_Menus_Listado(CadenaConexion);
            var result = await datos.ListadoMenus(idmodulo);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarID(int idmenu)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Menus_BuscarID datos = new AD_Menus_BuscarID(CadenaConexion);
            var result = await datos.BuscarID(idmenu);
            return Ok(result);

        }


    }
}
