using HD.Clientes.Consultas.SolicitudCreditoDocumento;
using HD.Notifications.NotificacionesApp;
using HD.Security;
using HD_Cobranza.Capturas.SolicitudCreditoDocumentacionCobranza;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class SolicitudCreditoDocumentacionCobranzaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SolicitudCreditoDocumentacionCobranzaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(DateTime fecha_inicio, DateTime fecha_fin, string sucursal, string adr, string tipo_solicitud)
        {
            if (string.IsNullOrWhiteSpace(sucursal)) sucursal = "0";
            if (string.IsNullOrWhiteSpace(adr)) adr = "0";
            if (string.IsNullOrWhiteSpace(tipo_solicitud)) tipo_solicitud = "T";

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_SolicitudCreditoDocumentacionCobranza datos = new AD_SolicitudCreditoDocumentacionCobranza(CadenaConexion);
            var result = await datos.Listado(fecha_inicio, fecha_fin, sucursal, adr, tipo_solicitud);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Detalle(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_SolicitudCreditoDocumentacionCobranza datos = new AD_SolicitudCreditoDocumentacionCobranza(CadenaConexion);
            var result = await datos.Detalle(folio);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerDocumento(string folio, int iddocumento)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADSolicitudCredito_Documentacion_ObtenerDocumento datos = new ADSolicitudCredito_Documentacion_ObtenerDocumento(CadenaConexion);
            var result = await datos.Obtener(folio, iddocumento);

            string origen = Sesion.origen();
            if (Sesion.generarLog() == true && origen == "APP")
            {
                NE_Logs_App_HD log = new NE_Logs_App_HD(CadenaConexion);
                await log.Guardar($"Descargo el documento {iddocumento} del folio: {folio}", origen, Sesion.usuario());
            }

            if (result is null)
                return BadRequest(new { mensaje = "Documento no encontrado. Favor de comunicarse con el administrador del sistema" });
            return Ok(result);
        }
    }
}
