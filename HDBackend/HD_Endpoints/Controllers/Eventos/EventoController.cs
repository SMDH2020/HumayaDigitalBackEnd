using HD.Clientes.Consultas.Cultivos;
using HD.Clientes.Consultas.Eventos;
using HD.Notifications.Consultas;
using HD.Security;
using HD_Dashboard.Consultas.Vendedor;
using HD_Dashboard.Modelos;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HD.Endpoints.Controllers.Eventos
{
    public class EventoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public EventoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Evento_Usuario_Listado datos = new AD_Evento_Usuario_Listado(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.Listado(usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Borrar(int idevento_usuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Evento_Usuario_Borrar_Notificaciones datos = new AD_Evento_Usuario_Borrar_Notificaciones(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.Borrar(idevento_usuario, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BorrarTodo()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Evento_Usuario_Borrar_Notificaciones datos = new AD_Evento_Usuario_Borrar_Notificaciones(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.BorrarTodo(usuario);
            return Ok(result);
        }
    }
}
