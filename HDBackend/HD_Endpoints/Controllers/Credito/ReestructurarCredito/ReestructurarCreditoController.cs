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
using HD.Clientes.Consultas.PedidoCondicionesCredito;
using HD.Clientes.Consultas.PedidoFinanciamiento;

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
        public async Task<ActionResult> ListadoCondiciones(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reestructura__Credito_Condiciones_Credito datos = new AD_Reestructura__Credito_Condiciones_Credito(CadenaConexion);
            var result = await datos.Obtener(folio);
            return Ok(result);

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarCondiciones(mdlPedido_Condiciones_Venta mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reestructura_Credito_Condiciones_Credito_Guardar datos = new AD_Reestructura_Credito_Condiciones_Credito_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            //AD_ClientesDatosPersonaFisica_Guardar datosfisica = new AD_ClientesDatosPersonaFisica_Guardar(CadenaConexion);
            //await datosfisica.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DetalleFinanciamiento(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reestructura_Credito_Detalle_Financiamiento_Info datos = new AD_Reestructura_Credito_Detalle_Financiamiento_Info(CadenaConexion);
            var result = await datos.Get(folio);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Delete(string folio, int docto)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reestructura_Credito_Detalle_Financiamiento_Borrar datos = new AD_Reestructura_Credito_Detalle_Financiamiento_Borrar(CadenaConexion);

            var result = await datos.Delete(folio, docto, Sesion.usuario());
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DeleteAll(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reestructura_Credito_Detalle_Financiamiento_Borrar datos = new AD_Reestructura_Credito_Detalle_Financiamiento_Borrar(CadenaConexion);

            var result = await datos.DeleteAll(folio);
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
            string OneSignalAppId = Configuracion["OneSignal:AppIDProduccion"];
            string OneSignalApiKey = Configuracion["OneSignal:ApyKeyproduccion"];
            AD_Reestructurar_Credito_Enviar_Revision datos = new AD_Reestructurar_Credito_Enviar_Revision(CadenaConexion);
            var result = await datos.Enviar_Solicitud(folio, Sesion.usuario());
            string mensaje = "Validación de condiciones en proceso";

            if (result != null)
            {
                await NSolicitud_Enviar.Enviar(result);

                //enviar notificacion
                var usuariosNotificados = string.Join(",", result.mdlSolicitud?.Select(u => u.idempleado.ToString()) ?? new List<string>());
                var usuario = Sesion.usuario();
                var textoCliente = string.IsNullOrWhiteSpace(result.detail?.razon_social)    ? "" : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.detail.razon_social.ToLower());

                var idevento = 4;
                var referencia = 9;

                AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
                var resultado = await usuarios.GuardarNotificacionSolicitud(idevento, referencia, "Se registro una reestructura para el cliente " + textoCliente, result.detail.folio_solicitud, usuariosNotificados);

                AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion, OneSignalAppId, OneSignalApiKey);
                await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");
            }

            var response = new mdlAnalisis_Mhusa_Resultado
            {
                socket = result.mdlSolicitud
            };

            return Ok(response);

        }

    }
}
