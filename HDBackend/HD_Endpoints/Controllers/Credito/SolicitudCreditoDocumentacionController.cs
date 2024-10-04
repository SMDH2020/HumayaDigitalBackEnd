using HD.Clientes.Consultas.AnalisisCredito.JDF_Condicionado;
using HD.Clientes.Consultas.Pagares;
using HD.Clientes.Consultas.PedidoImpresion;
using HD.Clientes.Consultas.SolicitudCreditoDocumento;
using HD.Clientes.Modelos;
using HD.Notifications.Analisis;
using HD.Security;
using HD_Reporteria.Pagares;
using HD_Reporteria;
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
        public async Task<ActionResult> ObtenerDocumentoResultadoOperacion(string folio, int iddocumento)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADSolicitudCredito_Documentacion_ObtenerDocumento datos = new ADSolicitudCredito_Documentacion_ObtenerDocumento(CadenaConexion);
            var result = await datos.ObtenerResultadoOperacion(folio, iddocumento);
            if (result is null)
                return BadRequest(new { mensaje = "Documento no encontrado. Favor de comunicarse con el administrador del sistema" });
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
        public async Task<ActionResult> ObtenerDocumentoValidacionF(string folio, int iddocumento)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Solicitud_Credito_Documentos_Validar_Factura datos = new AD_Solicitud_Credito_Documentos_Validar_Factura(CadenaConexion);
            var result = await datos.Obtener(folio, iddocumento);
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
                    var documento = resultpdf.condiciones.mhusajdf == "JDF" ? RPT_Pedido_JDF.Generar(resultpdf) : RPT_Pedido.Generar(resultpdf);
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

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerDocumentoPagare(string folio, int iddocumento)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADSolicitudCredito_Documentacion_ObtenerDocumento datos = new ADSolicitudCredito_Documentacion_ObtenerDocumento(CadenaConexion);
            var result = await datos.ObtenerPagare(folio, iddocumento);
            if (result is null)
            {
                AD_Pagare_Dos_Amortizaciones_Suscripcion datoss = new AD_Pagare_Dos_Amortizaciones_Suscripcion(CadenaConexion);
                var results = await datoss.Get(folio);
                List<HD_Reporteria.RPT_Result> documento = new List<HD_Reporteria.RPT_Result>();

                try
                {
                    // Agrupar por tasas diferentes
                    var gruposTasas = results.financiamientocerodias.GroupBy(f => f.tasa);
                    var gruposTasasmas = results.financiamientomasdias.GroupBy(f => f.tasa);


                    foreach (var grupo in gruposTasas)
                    {
                        if (grupo.Count() > 1)
                        {
                            HD_Reporteria.RPT_Result reporte = RPT_Pagare_Dos_Amortizaciones_Vencimiento.Generar(results, grupo.ToList());
                            documento.Add(reporte);
                        }
                        else
                        {
                            HD_Reporteria.RPT_Result reporte = RPT_Pagare_Vencimiento.Generar(results, grupo.First());
                            documento.Add(reporte);
                        }
                    }

                    foreach (var grupo in gruposTasasmas)
                    {
                        if (grupo.Count() > 1)
                        {
                            HD_Reporteria.RPT_Result reporte = RPT_Pagare_Dos_Amortizaciones_Suscripcion.Generar(results, grupo.ToList());
                            documento.Add(reporte);
                        }
                        else
                        {
                            HD_Reporteria.RPT_Result reporte = RPT_Pagare_Suscripcion.Generar(results, grupo.First());
                            documento.Add(reporte);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest("Error de servidor: " + ex.Message);
                }

                return Ok(documento);
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarDocumentoCondicionado(mdlSolicitudCredito_Documentacion_View mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADSolicitud_Credito_Documentacion_Condicionada_Guardar datos = new ADSolicitud_Credito_Documentacion_Condicionada_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarDocumentoAceptadoCondicionado(mdlSolicitudCredito_Documentacion_View mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADSolicitud_Credito_Documentacion_Condicionada_Guardar datos = new ADSolicitud_Credito_Documentacion_Condicionada_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarDocumentacionAceptada(mdl);
            if (result.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            if (result.completado.completado == 1)
            {
                await NotificacionComentarios.EnviarCargaDocumentosAprobadosCondicionado(result);
            }
            return Ok(result);

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarDocumentoJDF(mdlSolicitudCredito_Documentacion_View mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADSolicitud_Credito_Documentacion_JDF_Guardar datos = new ADSolicitud_Credito_Documentacion_JDF_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);

        }

    }
}
