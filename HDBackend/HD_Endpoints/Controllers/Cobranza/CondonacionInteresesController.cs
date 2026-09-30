using HD.Notifications;
using HD.Notifications.Modelos;
using HD.Security;
using HD_Cobranza.Capturas.CondonacionIntereses;
using HD_Cobranza.Capturas.ConvenioPago;
using HD_Cobranza.Modelos.CondonacionIntereses;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class CondonacionInteresesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CondonacionInteresesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_Guarda_Usuarios_Autorizan_Condonacion mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Autorizan_Condonacion_Intereses datos = new AD_Autorizan_Condonacion_Intereses(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Guardar(mdl);

            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Eliminar(mdl_Elimina_Usuario_Autoriza_Condonacion mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Autorizan_Condonacion_Intereses datos = new AD_Autorizan_Condonacion_Intereses(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Eliminar(mdl);

            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> UsuarioCondonacionID(int idAutoriza)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Autorizan_Condonacion_Intereses datos = new AD_Autorizan_Condonacion_Intereses(CadenaConexion);
            var result = await datos.ObtenerUsuarioCondonacionID(idAutoriza);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoUsuariosCondonacion()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Autorizan_Condonacion_Intereses datos = new AD_Autorizan_Condonacion_Intereses(CadenaConexion);
            var result = await datos.ListadoUsuarios();
            return Ok(result);

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerLimiteUsuario()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Condonacion_Intereses datos = new AD_Condonacion_Intereses(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerLimiteUsuario(usuario);

            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarCondonacion(mdl_Guarda_Condonacion_Interes mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            string OneSignalAppId = Configuracion["OneSignal:AppIDProduccion"];
            string OneSignalApiKey = Configuracion["OneSignal:ApyKeyproduccion"];
            AD_Condonacion_Intereses datos = new AD_Condonacion_Intereses(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Guardar(mdl);

            var condonacion = result.FirstOrDefault();

            // ------------------------------------------------------------
            // Notificación: solo si quedó pendiente y hay a quién avisar.
            // idencabezado=-99, evento=1 y usuarioNotificar en CSV, confirmados.
            // ------------------------------------------------------------
            if (condonacion != null
                && condonacion.Estatus == "P"
                && !string.IsNullOrWhiteSpace(condonacion.usuarios_notificar))
            {
                try
                {
                    AD_OneSignal adNotificaciones = new(CadenaConexion, OneSignalAppId, OneSignalApiKey);

                    var notificacion = new NotificacionDto
                    {
                        idencabezado = -99,
                        Titulo = "Condonación pendiente de autorización",
                        Mensaje = $"La condonación {condonacion.Folio} requiere tu autorización.",
                        redireccion = "12",
                        evento = 15,
                        fecha_evento = DateTime.Now,
                        usuario = mdl.usuario.ToString(),
                        usuarioNotificar = condonacion.usuarios_notificar,   // "9035,1111"
                        parametro = condonacion.Folio,
                    };

                    await adNotificaciones.EnviarEspecifico(notificacion);
                }
                catch (System.Exception ex)
                {
                    // No se detiene el guardado si falla el envío de la
                    // notificación (la condonación ya quedó guardada).
                    Console.WriteLine($"[GuardarCondonacion] Error notificación push: {ex}");
                }

                try
                {
                    string smtpHost = "correo.humaya.com.mx";
                    int smtpPort = 587;
                    bool smtpSecure = false;
                    string smtpUser = "HumayaDigital@humaya.com.mx";
                    string smtpPass = "!HD_Hum4y4D1g1t4l*T1?";

                    await datos.EnviarCorreoCondonacion(
                        condonacion.Folio,
                        "solicitud",
                        condonacion.correos_notificar,
                        smtpHost, smtpPort, smtpSecure, smtpUser, smtpPass);
                }
                catch (System.Exception ex)
                {
                    // No se detiene el guardado si falla el envío del correo.
                    Console.WriteLine($"[GuardarCondonacion] Error notificación correo: {ex}");
                }
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerCondonacionesCliente(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Condonacion_Intereses datos = new AD_Condonacion_Intereses(CadenaConexion);
            var result = await datos.ObtenerCondonacionesCliente(idcliente);

            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ValidarCondonacionesCliente(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Condonacion_Intereses datos = new AD_Condonacion_Intereses(CadenaConexion);
            var result = await datos.ValidarCondonacionesCliente(idcliente);

            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerCondonacionPorFolio(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Condonacion_Intereses datos = new AD_Condonacion_Intereses(CadenaConexion);
            var result = await datos.ObtenerCondonacionPorFolio(folio);

            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AutorizarCondonacion(mdl_Autorizar_Condonacion mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            string OneSignalAppId = Configuracion["OneSignal:AppIDProduccion"];
            string OneSignalApiKey = Configuracion["OneSignal:ApyKeyproduccion"];
            AD_Condonacion_Intereses datos = new AD_Condonacion_Intereses(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Autorizar(mdl);

            var autorizacion = result.FirstOrDefault();

            // ------------------------------------------------------------
            // Notificación: avisarle a quien capturó la condonación si
            // fue aprobada o rechazada, junto con el folio.
            // ------------------------------------------------------------
            if (autorizacion != null && autorizacion.idcreador.HasValue)
            {
                try
                {
                    AD_OneSignal adNotificaciones = new(CadenaConexion, OneSignalAppId, OneSignalApiKey);

                    string estadoTexto = autorizacion.Estatus == "A" ? "aprobada" : "rechazada";

                    var notificacion = new NotificacionDto
                    {
                        idencabezado = -99,
                        Titulo = $"Condonación {estadoTexto}",
                        Mensaje = $"Tu condonación fue {estadoTexto}.",
                        redireccion = "12",
                        evento = 15,
                        fecha_evento = DateTime.Now,
                        usuario = mdl.usuario.ToString(),
                        usuarioNotificar = autorizacion.idcreador.ToString(),
                        parametro = autorizacion.Folio,
                    };

                    await adNotificaciones.EnviarEspecifico(notificacion);
                }
                catch (System.Exception ex)
                {
                    // No se detiene la autorización si falla el envío de la
                    // notificación (el estatus ya quedó actualizado).
                    Console.WriteLine($"[AutorizarCondonacion] Error notificación push: {ex}");
                }

                try
                {
                    string smtpHost = "correo.humaya.com.mx";
                    int smtpPort = 587;
                    bool smtpSecure = false;
                    string smtpUser = "HumayaDigital@humaya.com.mx";
                    string smtpPass = "!HD_Hum4y4D1g1t4l*T1?";

                    string tipoEvento = autorizacion.Estatus == "A" ? "aprobada" : "rechazada";

                    await datos.EnviarCorreoCondonacion(
                        autorizacion.Folio!,
                        tipoEvento,
                        autorizacion.correo_creador,
                        smtpHost, smtpPort, smtpSecure, smtpUser, smtpPass);
                }
                catch (System.Exception ex)
                {
                    // No se detiene la autorización si falla el envío del correo.
                    Console.WriteLine($"[AutorizarCondonacion] Error notificación correo: {ex}");
                }
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerCondonacionesReporte(int ejercicio, int periodo, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Condonacion_Intereses datos = new AD_Condonacion_Intereses(CadenaConexion);
            var result = await datos.ObtenerCondonacionesReporte(ejercicio, periodo, adr, sucursal);
            return Ok(result);
        }
    }
}