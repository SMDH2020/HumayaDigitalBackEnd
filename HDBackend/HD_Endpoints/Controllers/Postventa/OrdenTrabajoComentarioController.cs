using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.OrdenesAbiertas;
using Postventa.Modelos.OrdenesAbiertas;

namespace HD.Endpoints.Controllers.Postventa
{
    public class OrdenTrabajoComentarioController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public OrdenTrabajoComentarioController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Categorias()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_OT_Comentario_Categorias_Listado datos = new AD_OT_Comentario_Categorias_Listado(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int OrdenTrabajoId)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_OT_Comentario_Listado datos = new AD_OT_Comentario_Listado(CadenaConexion);
            var result = await datos.Listado(OrdenTrabajoId);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_OT_Comentario_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_OT_Comentario_Guardar datos = new AD_OT_Comentario_Guardar(CadenaConexion);
            mdl.UsuarioRegistro = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }
    }
}
