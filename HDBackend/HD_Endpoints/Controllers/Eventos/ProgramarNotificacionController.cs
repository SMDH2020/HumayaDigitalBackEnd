using HD.Clientes.Consultas.Cultivos;
using HD.Clientes.Modelos;
using HD.Notifications.Consultas;
using HD.Notifications.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Usados.Consultas.Inventario;
using Usados.Modelos.Inventario;
using HD.Notifications;

namespace HD.Endpoints.Controllers.Eventos
{
    public class ProgramarNotificacionController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public ProgramarNotificacionController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }


        private const string OneSignalAppId = "04e611d6-045a-4105-af2d-04880d3c4cb9"; // Tu App ID
        private const string OneSignalApiKey = ""; // ⚠️ Tu REST API Key

        [HttpPost]
        public async Task<ActionResult> Post(mdl_HD_Notificaciones mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_HD_Notificaciones_Guardar datos = new AD_HD_Notificaciones_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarInstantanea(mdl_HD_Notificaciones mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_HD_Notificaciones_Guardar datos = new AD_HD_Notificaciones_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result =  await datos.GuardarInstantanea(mdl);

            //enviar notificacion
            DateTime fecha_evento = DateTime.Now;

            AD_OneSignal usuarios = new AD_OneSignal(CadenaConexion);
            await usuarios.EnviarTodos(result.idencabezado, fecha_evento, mdl.usuario);

            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_HD_Notificaciones_Listado datos = new AD_HD_Notificaciones_Listado(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoFiltrado(string? iddepartamento, string? tipo, DateTime fecha_inicio, DateTime fecha_fin)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_HD_Notificaciones_Listado_Filtrado datos = new AD_HD_Notificaciones_Listado_Filtrado(CadenaConexion);
            var result = await datos.Listado( iddepartamento, tipo, fecha_inicio, fecha_fin);
            return Ok(result);
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado_Detalle(int idencabezado)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_HD_Notificaciones_ObtenerporID datos = new AD_HD_Notificaciones_ObtenerporID(CadenaConexion);
            var result = await datos.obtenerListadoDetalle(idencabezado);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarID(int iddetalle)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_HD_Notificaciones_ObtenerporID datos = new AD_HD_Notificaciones_ObtenerporID(CadenaConexion);
            var result = await datos.obtenerID(iddetalle);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> PausarNotificacion(int idencabezado, bool estatus)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            HD_Notificaciones_Pausar_Notificacion datos = new HD_Notificaciones_Pausar_Notificacion(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.Pausar(idencabezado, estatus, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EliminarNotificacion(int idencabezado)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_HD_Notificaciones_Eliminar_Notificacion datos = new AD_HD_Notificaciones_Eliminar_Notificacion(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.Eliminar(idencabezado);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Modulos_Redireccion()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Modulos_Redireccion_Listado datos = new AD_Modulos_Redireccion_Listado(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> cambiar_estado(int idencabezado)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_HD_Notificaciones_CambiarEstado datos = new AD_HD_Notificaciones_CambiarEstado(CadenaConexion);
            var result = await datos.cambiarEstado(idencabezado);
            return Ok(new { mensaje = "datos cambiados con exito" });
        }
    }
}
