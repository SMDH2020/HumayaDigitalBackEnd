using HD.Generales.Autenticate;
using HD.Generales.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Authenticate
{
    public class LineasVentasUsuariosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public LineasVentasUsuariosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Post(mdl_Linea_Venta_Usuario_Permiso mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RelUsuarioLineaVenta_Guardar datos = new AD_RelUsuarioLineaVenta_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoLineas()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Lineas_Venta_Usuarios_Listado datos = new AD_Lineas_Venta_Usuarios_Listado(CadenaConexion);
            var result = await datos.ListadoLineas();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoUsuarioLineasVenta(int idlinea, int idusuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RelUsuariosLineasVenta_Listado datos = new AD_RelUsuariosLineasVenta_Listado(CadenaConexion);
            var result = await datos.ListadoLineasVentaRel(idlinea, idusuario);
            return Ok(result);
        }


    }
}
