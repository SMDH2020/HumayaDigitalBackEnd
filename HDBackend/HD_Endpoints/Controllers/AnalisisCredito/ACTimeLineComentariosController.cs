using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Consultas.AnalisisCredito.JDF_Condicionado;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Notifications.Analisis;
using HD.Notifications.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
            if (mdl.idproceso == 10)
            {
                ADAnalisisNotificacionFacturacion notificacion = new ADAnalisisNotificacionFacturacion(CadenaConexion);
                var body = await notificacion.GetBody(mdl);
                await NotificacionComentarios.Enviar(body);
                var response = new mdlAnalisis_Mhusa_Resultado
                {
                    estado = result.estado,
                    socket = result.mdlSolicitud
                };
                return Ok(response);
            }
            else
            {
                if (result.mdldatos is null)
                {
                    return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
                }
                await NotificacionComentarios.Enviar_Mhusa(result);

                if(mdl.idproceso == 29 || mdl.idproceso == 38 || mdl.idproceso == 1520 )
                {
                    //enviar notificacion
                    var usuariosNotificados = string.Join(",", result.mdlSolicitud.Select(u => u.idempleado.ToString()) ?? new List<string>());
                    var usuario = Sesion.usuario();
                    var textoCliente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.mdldatos.cliente.ToLower());
                    var idevento = mdl.folio.Substring(0, 2) == "PC" ? 3 : mdl.folio.Substring(0, 2) == "RC" ? 4 : 1;
                    var referencia = 9;
                    string mensaje;

                    if(mdl.idproceso == 38 || mdl.idproceso == 1520) {
                        mensaje = "Se finalizo timeline de " + textoCliente;
                    }
                    else
                    {
                        mensaje = "Gerente valido el pedido de " + textoCliente;
                    }

                    AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
                    var resultado = await usuarios.GuardarNotificacionSolicitud(idevento, referencia, mensaje, mdl.folio, usuariosNotificados);

                    AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion);
                    await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");
                }

                var response = new mdlAnalisis_Mhusa_Resultado
                {
                    estado = result.estado,
                    socket = result.mdlSolicitud
                };
                return Ok(response);
            }
            //ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            //return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EnviarModificar(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Comentarios datos = new ADAnalisis_Comentarios(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.EnviarModificacion(mdl);
            if (result is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            if (mdl.idproceso == 10)
            {
                ADAnalisisNotificacionFacturacion notificacion = new ADAnalisisNotificacionFacturacion(CadenaConexion);
                var body = await notificacion.GetBody(mdl);
                await NotificacionComentarios.Enviar(body);
                var response = new mdlAnalisis_Mhusa_Resultado
                {
                    estado = result.estado,
                    socket = result.mdlSolicitud
                };
                return Ok(response);
            }
            else
            {
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


                AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion);
                await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");

                var response = new mdlAnalisis_Mhusa_Resultado
                {
                    estado = result.estado,
                    socket = result.mdlSolicitud
                };
                return Ok(response);
            }
            //ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            //return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> OtorgamientoCredito(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Comentarios datos = new ADAnalisis_Comentarios(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarOtorgamiento(mdl);
            if (result is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }

            await NotificacionComentarios.Enviar_Mhusa(result);
            return Ok(new
                {
                    estado = result.estado,
                    socket = result.mdlSolicitud
                }
            );
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarEnganche(mdlEnchanche_Mhusa mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADSolicitud_Credito_Enganche_Guardar datos = new ADSolicitud_Credito_Enganche_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            if (result is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            else
            {
                if (result.mdldatos is null)
                {
                    return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
                }
                await NotificacionComentarios.Enviar_Mhusa(result);
                return Ok(new
                {
                    estado = result.estado,
                    socket = result.mdlSolicitud
                }
 );
            }
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarComentario(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Comentarios_JDF_Condicionado datos = new ADAnalisis_Comentarios_JDF_Condicionado(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);


            if (result is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            if (mdl.idproceso == 10)
            {
                ADAnalisisNotificacionFacturacion notificacion = new ADAnalisisNotificacionFacturacion(CadenaConexion);
                var body = await notificacion.GetBody(mdl);
                await NotificacionComentarios.Enviar(body);
                var response = new mdlAnalisis_Mhusa_Resultado
                {
                    estado = result.estado,
                    socket = result.mdlSolicitud
                };
                return Ok(response);
            }
            else
            {
                if (result.mdldatos is null)
                {
                    return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
                }
                await NotificacionComentarios.Enviar_Mhusa(result);


                if (mdl.idproceso == 35 || mdl.idproceso == 1100 || mdl.idproceso == 1010|| mdl.idproceso == 1150)
                {
                    //enviar notificacion
                    var usuariosNotificados = string.Join(",", result.mdlSolicitud?.Select(u => u.idempleado.ToString()) ?? new List<string>());
                    var usuario = Sesion.usuario();
                    var textoCliente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.mdldatos.cliente.ToLower());
                    var idevento = 1;
                    var referencia = mdl.idproceso == 1010 ? 9 : 11;

                    string mensaje;

                    if(mdl.idproceso == 1010)
                    {
                        mensaje = "Pedido autorizado del cliente " + textoCliente;
                    }
                    else if(mdl.idproceso == 1150)
                    {
                        mensaje = "Se finalizo timeline de " + textoCliente;
                    }
                    else
                    {
                        mensaje= "Facturación autorizada para " + textoCliente;
                    }
                    AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
                    var resultado = await usuarios.GuardarNotificacionSolicitud(idevento, referencia, mensaje,mdl.folio, usuariosNotificados);

                    AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion);
                    await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");
                }

                var response = new mdlAnalisis_Mhusa_Resultado
                {
                    estado = result.estado,
                    socket = result.mdlSolicitud
                };
                return Ok(response);
            }
            //ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            //return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarPrecalificacion(mdlSCAnalisis_Comentarios_Precalificacion mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Comentarios_JDF_Condicionado datos = new ADAnalisis_Comentarios_JDF_Condicionado(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarPrecalificacion(mdl);
            if (result is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            if (mdl.idproceso == 10)
            {
                ADAnalisisNotificacionFacturacion notificacion = new ADAnalisisNotificacionFacturacion(CadenaConexion);
                var body = await notificacion.GetBodyPrecalificacion(mdl);
                await NotificacionComentarios.Enviar(body);
                var response = new mdlAnalisis_Mhusa_Resultado
                {
                    estado = result.estado,
                    socket = result.mdlSolicitud
                };
                return Ok(response);
            }
            else
            {
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
                return Ok(response);
            }
            //ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            //return Ok(result);
        }
    }
}
