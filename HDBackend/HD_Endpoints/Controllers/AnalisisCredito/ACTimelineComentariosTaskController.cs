using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Notifications.Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito
{
    public class ACTimelineComentariosTaskController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACTimelineComentariosTaskController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlSCAnalisisComentariosTask mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Comentarios_Task datos = new ADAnalisis_Comentarios_Task(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);

            bool notificar = result.documentacion.All(item => item.icono != "wait");

            if (notificar)
            {
                ADAnalisisSolicitudNotificacion notificacion = new ADAnalisisSolicitudNotificacion(CadenaConexion);
                var body = await notificacion.GetBody(mdl);
                await NotificacionDocumentacion.Enviar(body,mdl.folio);
            }

            return Ok(result);

        }
        [Route("/api/[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult> EnviarComentario(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisiCreditoMhusa datos = new ADAnalisiCreditoMhusa(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            if (result.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            if (result.mdldatos.noificar == true) await NotificacionComentarios.Enviar_Mhusa(result);
            return Ok(new
            {
                documentacion= result.documentacion,
                estado= result.estado
            });

            //ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            //return Ok(result);
        }

        
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EnviarComentarioCondicionado(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisiCreditoMhusa datos = new ADAnalisiCreditoMhusa(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarComentarioCondicionado(mdl);
            if (result.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            if (result.mdldatos.noificar == true) await NotificacionComentarios.Enviar_Mhusa(result);
            return Ok(new
            {
                documentacion = result.documentacion,
                estado = result.estado
            });

            //ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            //return Ok(result);
        }

        [Route("/api/[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult> EnviarComentarioOtorgamiento(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisiCreditoMhusa datos = new ADAnalisiCreditoMhusa(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarOtorgamiento(mdl);
            if (result.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            await NotificacionComentarios.Enviar_Mhusa(result);
            return Ok(new
            {
                documentacion = result.documentacion,
                estado = result.estado
            });

            //ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            //return Ok(result);
        }


        [Route("/api/[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult> EnviarComentarioOtorgamientoCondicionado(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisiCreditoMhusa datos = new ADAnalisiCreditoMhusa(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarOtorgamientoComentariosCondicionado(mdl);

            //await NotificacionComentarios.Enviar_Mhusa(result);
            return Ok(result);

            //ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            //return Ok(result);
        }
    }
}
