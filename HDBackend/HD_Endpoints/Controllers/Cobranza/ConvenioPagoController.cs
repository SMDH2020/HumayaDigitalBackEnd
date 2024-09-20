using HD.Clientes.Consultas.InteresMensual;
using HD.Clientes.Consultas.PedidoImpresion;
using HD.Clientes.Consultas.PedidoUnidades;
using HD.Clientes.Modelos;
using HD.Generales.Consultas;
using HD.Security;
using HD_Cobranza.Capturas.ConvenioPago;
using HD_Cobranza.Modelos.ConvenioPago;
using HD_Reporteria;
using HD_Reporteria.Cobranza;
using HD_Reporteria.Solicitud_Credito;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class ConvenioPagoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ConvenioPagoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdlConvenio_Pago mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADConvenio_Pago datos = new ADConvenio_Pago(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            try
            {
                RPT_Result documento = RPT_ConvenioPago.Generar(mdl, result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteConvenioPDF(mdlConvenio_Pago mdl )
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];

            try
            {
                //ADFacturasSeleccionadas datos = new ADFacturasSeleccionadas(CadenaConexion);
                //IEnumerable<mdlFacturasSeleccionadas> result;
                //if (mdl.tipo_credito == "O")
                //    //result = await datos.ObtenerOperacion(mdl.idcliente);
                //    result = await datos.ObtenerOperacion(mdl.idcliente);

                //else
                //    //result = await datos.ObtenerRevolventeob(mdl.idcliente);
                //    result = await datos.ObtenerOperacion(mdl.idcliente);

                IEnumerable<mdlFacturasSeleccionadas> result = JsonConvert.DeserializeObject<IEnumerable<mdlFacturasSeleccionadas>>(mdl.detalle);
                RPT_Result documento = RPT_ConvenioPago.Generar(mdl,result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error de servidor: {ex.Message}");

            }

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADConvenioListado datos = new ADConvenioListado(CadenaConexion);
            var result = await datos.Get(idcliente);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReportePDF(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADConvenio_Pago_Impresion datos = new ADConvenio_Pago_Impresion(CadenaConexion);
            var result = await datos.Get(folio);
            try
            {
                RPT_Result documento = RPT_ConvenioPago.GenerarPDF(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CargarEvidencia(mdlConvenio_Pago mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADEvidencia_Convenio_Pago_Guardar datos = new ADEvidencia_Convenio_Pago_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito", listado = result });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerDocumento(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADEvidencia_Convenio_Pago_ObtenerDocumento datos = new ADEvidencia_Convenio_Pago_ObtenerDocumento(CadenaConexion);
            var result = await datos.Obtener(folio);
            if (string.IsNullOrEmpty(result.documento))
            {
                return BadRequest(new { mensaje = "Documento aun no cargado. Favor de cargarlo primero" });
            }
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownList()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADResponsables_Cobranza_Dropdownlist datos = new ADResponsables_Cobranza_Dropdownlist(CadenaConexion);
            var result = await datos.DropDownList();
            return Ok(result);

        }
    }
}
