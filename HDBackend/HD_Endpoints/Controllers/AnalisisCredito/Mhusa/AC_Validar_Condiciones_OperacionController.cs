using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Notifications.Analisis;
using HD.Notifications.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Sockets;

namespace HD.Endpoints.Controllers.AnalisisCredito.Mhusa
{
    public class AC_Validar_Condiciones_OperacionController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public AC_Validar_Condiciones_OperacionController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ValidarCondicionesOperacion(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisiCreditoMhusa datos = new ADAnalisiCreditoMhusa(CadenaConexion);
            var result = await datos.ValidarCondicionesOperacion(folio, Sesion.usuario());
            return Ok(result);

        }

        [Route("/api/[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult> EnviarComentario(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            string OneSignalAppId = Configuracion["OneSignal:AppIDProduccion"];
            string OneSignalApiKey = Configuracion["OneSignal:ApyKeyproduccion"];
            ADAnalisiCreditoMhusa datos = new ADAnalisiCreditoMhusa(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarValidacion(mdl);
            if (result.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            await NotificacionComentarios.Enviar_Mhusa(result);

            var response = new mdlAnalisis_Mhusa_Resultado
            {
                estado = result.estado,
                socket = result.mdlSolicitud
            };

            //enviar notificacion
            var usuariosNotificados = string.Join(",", result.mdlSolicitud.Select(u => u.idempleado.ToString()) ?? new List<string>());
            var usuario = Sesion.usuario();
            var textoCliente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.mdldatos.cliente.ToLower());
            var idevento = 1;
            var referencia = 9;
            string mensaje;
            if (mdl.estatus == "M")
            {
                mensaje = "Se solicita modificar pedido de " + textoCliente;
            }
            else
            {
                mensaje = "Se aceptaron las condiciones del cliente " + textoCliente;
            }

            AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
            var resultado = await usuarios.GuardarNotificacionSolicitud(idevento, referencia, mensaje,mdl.folio.Substring(0, 2) == "CC" ? result.mdldatos.folio :  mdl.folio, usuariosNotificados);

            AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion, OneSignalAppId, OneSignalApiKey);
            await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");

            return Ok(response);
            
            //ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            //return Ok(result);
        }

        [Route("/api/[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult> EnviarModificacion(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            string OneSignalAppId = Configuracion["OneSignal:AppIDProduccion"];
            string OneSignalApiKey = Configuracion["OneSignal:ApyKeyproduccion"];
            ADAnalisiCreditoMhusa datos = new ADAnalisiCreditoMhusa(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.EnviarModificacion(mdl);
            if (result.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            await NotificacionComentarios.Enviar_Mhusa(result);

            //enviar notificacion
            var usuariosNotificados = string.Join(",", result.mdlSolicitud?.Select(u => u.idempleado.ToString()) ?? new List<string>());
            var usuario = Sesion.usuario();
            var textoCliente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.mdldatos.cliente.ToLower());
            var idevento = 1;
            var referencia = 9;

            AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
            var resultado = await usuarios.GuardarNotificacionSolicitud(idevento, referencia, "Modificacion de pedido de " + textoCliente, mdl.folio, usuariosNotificados);

            AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion, OneSignalAppId, OneSignalApiKey);
            await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");

            var response = new mdlAnalisis_Mhusa_Resultado
            {
                estado = result.estado,
                socket = result.mdlSolicitud
            };
            return Ok(response);

            //ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            //return Ok(result);
        }
    }
}
