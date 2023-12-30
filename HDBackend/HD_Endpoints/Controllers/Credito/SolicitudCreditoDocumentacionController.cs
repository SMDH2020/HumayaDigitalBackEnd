using HD.Clientes.Consultas.SolicitudCreditoDocumento;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class SolicitudCreditoDocumentacionController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SolicitudCreditoDocumentacionController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlSolicitudCredito_Documentacion_View mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADSolicitudCredito_Documentacion_Guardar datos = new ADSolicitudCredito_Documentacion_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> OBtenerListado(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADSolicitudCredito_Documentacion_Obtener datos = new ADSolicitudCredito_Documentacion_Obtener(CadenaConexion);
            var result = await datos.Obtener(folio);
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerDocumento(string folio, int iddocumento)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADSolicitudCredito_Documentacion_ObtenerDocumento datos = new ADSolicitudCredito_Documentacion_ObtenerDocumento(CadenaConexion);
            var result = await datos.Obtener(folio, iddocumento);
            if (result is null)
                return BadRequest("Documento no encontrado");
            return Ok(result);

        }
    }
}
