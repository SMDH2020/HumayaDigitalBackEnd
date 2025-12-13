using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Notifications.Analisis;
using HD.Notifications.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Cryptography.Xml;

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
        public async Task<ActionResult> EnviarAnalisisDocumentacion(mdl_Analisis_Documentacion mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisiCreditoMhusa datos = new ADAnalisiCreditoMhusa(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarAnalisis(mdl);
            if (result.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            if (result.mdldatos.noificar == true) await NotificacionComentarios.Enviar_Mhusa(result);

            var tipoSolictitud = mdl.folio.Substring(0, 2);
            var idevento = mdl.folio.Substring(0, 2) == "PC" ? 3 : 1;
            var referencia = 9;

            if ( mdl.idproceso == 31 && result.mdldatos.noificar == true)
            {
                //enviar notificacion
                var usuariosNotificados = string.Join(",", result.mdlSolicitud?.Select(u => u.idempleado.ToString()) ?? new List<string>());
                var usuario = Sesion.usuario();
                var textoCliente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.mdldatos.cliente.ToLower());

                AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
                var resultado = await usuarios.GuardarNotificacionSolicitud(idevento, referencia, "Se aprobo toda la documentación del cliente " + textoCliente, mdl.folio, usuariosNotificados);


                AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion);
                await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");
            }

            if (mdl.estatus == "M") {
                //enviar notificacion
                var usuariosNotificados = string.Join(",", result.mdlSolicitud?.Select(u => u.idempleado.ToString()) ?? new List<string>());
                var usuario = Sesion.usuario();
                var textoCliente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.mdldatos.cliente.ToLower());

                AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
                var resultado = await usuarios.GuardarNotificacionSolicitud(idevento, referencia, "Modificar " + mdl.nombreDocumento.ToLower() + " del cliente " + textoCliente, mdl.folio, usuariosNotificados);

                AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion);
                await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");
            }


            return Ok(new
            {
                documentacion = result.documentacion,
                estado = result.estado,
                socket = result.mdlSolicitud
            });

            //ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            //return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EnviarAnalisisDocumentacionCondicionada(mdl_Analisis_Documentacion mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisiCreditoMhusa datos = new ADAnalisiCreditoMhusa(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarAnalisisCondicionado(mdl);
            if (result.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            if (result.mdldatos.noificar == true) await NotificacionComentarios.Enviar_Mhusa(result);
            if (result.mdldatos.noificar != true)
            {
                // Crear un solo objeto mdlSolicitud con idusuario igual a 0
                result.mdlSolicitud = new List<mdlSolicitudCredito_Enviar>
                      {
                        new mdlSolicitudCredito_Enviar {
                            idempleado = 0,
                            nombre = "",
                            correo = ""
                        }

                      };
            }
            return Ok(new
            {
                documentacion = result.documentacion,
                estado = result.estado,
                socket = result.mdlSolicitud
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

            //enviar notificacion
            var usuariosNotificados = string.Join(",", result.mdlSolicitud?.Select(u => u.idempleado.ToString()) ?? new List<string>());
            var usuario = Sesion.usuario();
            var textoCliente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.mdldatos.cliente.ToLower());

            //AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
            //var resultado = await usuarios.GuardarNotificacionSolicitud(mdl.folio, "Se aprobo " + mdl.nombreDocumento.ToLower() + " del cliente " + textoCliente, 9, usuario, usuariosNotificados);

            //AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion);
            //await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");

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
        public async Task<ActionResult> EnviarComentarioOtorgamientoVencimiento(mdl_Analisis_Documentacion mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisiCreditoMhusa datos = new ADAnalisiCreditoMhusa(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarOtorgamientoVencimiento(mdl);
            if (result.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            await NotificacionComentarios.Enviar_Mhusa(result);
            //if (result.mdldatos.noificar != true)
            //{
            //    // Crear un solo objeto mdlSolicitud con idusuario igual a 0
            //    result.mdlSolicitud = new List<mdlSolicitudCredito_Enviar>
            //    {
            //            new mdlSolicitudCredito_Enviar {
            //                idempleado = 0,
            //                nombre = "",
            //                correo = ""
            //            }

            //    };
            //}

            //enviar notificacion
            var usuariosNotificados = string.Join(",", result.mdlSolicitud?.Select(u => u.idempleado.ToString()) ?? new List<string>());
            var usuario = Sesion.usuario();
            var textoCliente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.mdldatos.cliente.ToLower());

            //AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
            //var resultado = await usuarios.GuardarNotificacionSolicitud(mdl.folio, "Se aprobo " + mdl.nombreDocumento.ToLower() + " del cliente " + textoCliente, 9, usuario, usuariosNotificados);

            //AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion);
            //await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");

            return Ok(new
            {
                documentacion = result.documentacion,
                estado = result.estado,
                socket = result.mdlSolicitud
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

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EnviarComentarioDocumentacionAceptadaCondicionado(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisiCreditoMhusa datos = new ADAnalisiCreditoMhusa(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarAnalisisDocumentosAceptadosCondicionados(mdl);
            if (result.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            if(result.completado.modificacion == 1 || result.completado.completado == 1)
            {
                await NotificacionComentarios.EnviarModificacionDocumentosAprobadosCondicionado(result);
            }
            if (result.completado.completado == 0)
            {
                // Crear un solo objeto mdlSolicitud con idusuario igual a 0
                result.mdlSolicitud = new List<mdlSolicitudCredito_Enviar>
                      {
                        new mdlSolicitudCredito_Enviar {
                            idempleado = 0,
                            nombre = "",
                            correo = ""
                        }

                      };
            }
            return Ok(result);
        }
    }
}
