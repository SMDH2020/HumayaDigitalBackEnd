using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Notifications.Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito
{
    public class ACTimeLineComentariosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACTimeLineComentariosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Comentarios datos = new ADAnalisis_Comentarios(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            if(result is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            if (result.idproceso == 10)
            {
                ADAnalisisNotificacionFacturacion notificacion = new ADAnalisisNotificacionFacturacion(CadenaConexion);
                var body = await notificacion.GetBody(mdl);
                await NotificacionComentarios.EnviarNotificacionFacturacion(body);
                return Ok(result);
            }
            else
            {
                ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
                var body = await notificacion.GetBody(mdl);
                await NotificacionComentarios.Enviar(body);
                return Ok(result);
            }
            //ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            //return Ok(result);
        }


    }
}
