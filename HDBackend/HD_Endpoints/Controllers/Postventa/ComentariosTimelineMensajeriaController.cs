using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.Dashboard;
using Postventa.Modelos;

namespace HD.Endpoints.Controllers.Postventa
{
    public class ComentariosTimelineMensajeriaController : MyBase
    {

        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ComentariosTimelineMensajeriaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idcomentario, string tipo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Comentario_Timeline_Mensajeria_Listado datos = new AD_Comentario_Timeline_Mensajeria_Listado(CadenaConexion);
            var result = await datos.Listado(idcomentario, tipo);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_Comentarios_Timeline_Mensajeria_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Comentarios_Mensajeria_Timeline_Guardar datos = new AD_Comentarios_Mensajeria_Timeline_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.guardar(mdl);
            return Ok(result);
        }
    }
}
