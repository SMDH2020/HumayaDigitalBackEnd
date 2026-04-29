using HD.Clientes.Consultas.Eventos;
using HD.Security;
using HD_Mensajeria.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Mensajeria
{
    public class NotificacionesWhatsappController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public NotificacionesWhatsappController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> obtenerNotificaciones()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Notificaciones_Whatsapp_Obtener_Listado datos = new AD_Notificaciones_Whatsapp_Obtener_Listado(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.obtenerNotificaciones(usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> borrarNotificacion(int idnotificacion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Notificaciones_Whatsapp_Borrar_Notificacion datos = new AD_Notificaciones_Whatsapp_Borrar_Notificacion(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.Borrar(idnotificacion, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> borrarTodo()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Notificaciones_Whatsapp_Borrar_Notificacion datos = new AD_Notificaciones_Whatsapp_Borrar_Notificacion(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.BorrarTodo(usuario);
            return Ok(result);
        }
    }
}
