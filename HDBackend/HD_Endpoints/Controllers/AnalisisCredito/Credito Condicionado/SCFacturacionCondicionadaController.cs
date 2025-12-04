using HD.Clientes.Consultas.Credito_Condicionado;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using HD.Notifications.Analisis;
using HD.Notifications.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HD.Endpoints.Controllers.AnalisisCredito.Credito_Condicionado
{
    public class SCFacturacionCondicionadaController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SCFacturacionCondicionadaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Enviar(mdl_Autorizar_facturacion_View mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            mdl.usuario = Sesion.usuario();
            AD_Credito_Autorizar_Facturacion_Condicionada da = new AD_Credito_Autorizar_Facturacion_Condicionada(CadenaConexion);
            var result = await da.Guardar(mdl);

            AD_Credito_Condicionado_Notificacion_Correo correo = new AD_Credito_Condicionado_Notificacion_Correo(CadenaConexion);
            var resultado  = await correo.Notificacion(mdl.folio,mdl.usuario,mdl.comentarios, 150);

            if (resultado.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            await NotificacionComentarios.EnviarNotificacionOperacionCondicionada(resultado);

            //enviar notificacion
            var usuariosNotificados = string.Join(",", resultado.mdlSolicitud.Select(u => u.idempleado.ToString()) ?? new List<string>());
            var usuario = Sesion.usuario();
            var textoCliente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(resultado.mdldatos.cliente.ToLower());
            var idevento = mdl.folio.Substring(0, 2) == "PC" ? 3 : mdl.folio.Substring(0, 2) == "CC" ? 2 : 1;
            var referencia = 11;

            AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
            var resultadoo = await usuarios.GuardarNotificacionSolicitud(idevento, referencia, "Credito autorizado para facturacion de " + textoCliente, resultado.mdldatos.folio_solicitud, usuariosNotificados);

            AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion);
            await notificacionPush.Enviar_Notificacion_Solicitud(resultadoo, "Humaya Digital");

            return Ok(new
            {
                detalle = result,
                socket = resultado.mdlSolicitud
            });
        }
    }
}
