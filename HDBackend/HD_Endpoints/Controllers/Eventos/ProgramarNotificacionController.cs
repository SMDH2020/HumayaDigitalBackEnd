using HD.Clientes.Consultas.Cultivos;
using HD.Clientes.Modelos;
using HD.Notifications.Consultas;
using HD.Notifications.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Usados.Consultas.Inventario;
using Usados.Modelos.Inventario;

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


        [HttpPost]
        public async Task<ActionResult> Post(mdl_HD_Notificaciones mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_HD_Notificaciones_Guardar datos = new AD_HD_Notificaciones_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
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
        public async Task<ActionResult> BuscarID(int idnotificacion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_HD_Notificaciones_ObtenerporID datos = new AD_HD_Notificaciones_ObtenerporID(CadenaConexion);
            var result = await datos.obtenerID(idnotificacion);
            return Ok(result);
        }
    }
}
