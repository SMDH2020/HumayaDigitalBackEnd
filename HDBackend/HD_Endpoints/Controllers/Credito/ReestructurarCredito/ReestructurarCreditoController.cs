using HD.Clientes.Consultas.PrestamoClientes;
using HD.Clientes.Modelos.PrestamoClientes;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD.Clientes.Consultas.ReestructurarCredito;
using HD.Clientes.Consultas.SolicitudCredito;
using HD.Notifications.Analisis;
using HD.Notifications.Consultas;
using System.Globalization;

namespace HD.Endpoints.Controllers.Credito.ReestructurarCredito
{
    public class ReestructurarCreditoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ReestructurarCreditoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoSolicitudes(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reestructurar_Credito_Listado_Solicitudes datos = new AD_Reestructurar_Credito_Listado_Solicitudes(CadenaConexion);
            var usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(usuario, idcliente);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DatosSolicitud(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reestructurar_Credito_Informacion_Solicitud datos = new AD_Reestructurar_Credito_Informacion_Solicitud(CadenaConexion);
            var result = await datos.Obtener(folio);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CrearReestructuracion(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reestructurar_Credito_Crear_Folio datos = new AD_Reestructurar_Credito_Crear_Folio(CadenaConexion);
            var usuario = int.Parse(Sesion.usuario());
            var result = await datos.Crear(folio,usuario);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> enviarRevision(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reestructurar_Credito_Enviar_Revision datos = new AD_Reestructurar_Credito_Enviar_Revision(CadenaConexion);
            var result = await datos.Enviar_Solicitud(folio, Sesion.usuario());
            string mensaje = "Validación de condiciones en proceso";

            if (result != null)
            {
                await NSolicitud_Enviar.Enviar(result);

                //enviar notificacion
                var usuariosNotificados = string.Join(",", result.mdlSolicitud?.Select(u => u.idempleado.ToString()) ?? new List<string>());
                var usuario = Sesion.usuario();
                var textoCliente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.detail.razon_social.ToLower());

                //AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
                //var resultado = await usuarios.GuardarNotificacionSolicitud(folio, "Se registro una reestructura para el cliente " + textoCliente, 9, usuario, usuariosNotificados);

                //AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion);
                //await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");
            }

            var response = new mdlAnalisis_Mhusa_Resultado
            {
                socket = result.mdlSolicitud
            };

            return Ok(response);

        }

    }
}
