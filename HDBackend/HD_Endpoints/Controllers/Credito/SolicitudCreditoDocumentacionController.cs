using HD.Clientes.Consultas.PedidoImpresion;
using HD.Clientes.Consultas.SolicitudCreditoDocumento;
using HD.Clientes.Modelos;
using HD.Security;
using HD_Reporteria.Solicitud_Credito;
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
                return BadRequest(new {mensaje= "Documento no encontrado. Favor de comunicarse con el administrador del sistema" });
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerFactura(string folio,int registro, int iddocumento)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADSolicitudCredito_Documentacion_ObtenerDocumento datos = new ADSolicitudCredito_Documentacion_ObtenerDocumento(CadenaConexion);
            var result = await datos.ObtenerFactura(folio,registro, iddocumento);
            if (result is null)
                return BadRequest(new { mensaje = "Documento no encontrado. Favor de comunicarse con el administrador del sistema" });
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerDocumentoPedido(string folio, int iddocumento)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADSolicitudCredito_Documentacion_ObtenerDocumento datos = new ADSolicitudCredito_Documentacion_ObtenerDocumento(CadenaConexion);
            var result = await datos.ObtenerPEdido(folio, iddocumento);
            if (result is null)
            {
                ADPedido_Impresion_View pdf = new ADPedido_Impresion_View(CadenaConexion);
                var resultpdf = await pdf.Get(folio);
                if (resultpdf.condiciones is null || resultpdf.condiciones is null  || resultpdf.unidades.Count == 0)
                {
                    return BadRequest(new { mensaje = "Para poder imprimir el Pedido es necesario completar toda la información solicitada" });
                }

                try
                {
                    var documento = resultpdf.condiciones.mhusajdf == "JDT" ? RPT_Pedido.Generar(resultpdf) : RPT_Pedido_JDF.Generar(resultpdf);
                    documento.documento = "data:application/pdf;base64," + documento.documento;
                    return Ok(documento);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error de servidor");

                }
            }
            return Ok(result);


        }

    }
}
