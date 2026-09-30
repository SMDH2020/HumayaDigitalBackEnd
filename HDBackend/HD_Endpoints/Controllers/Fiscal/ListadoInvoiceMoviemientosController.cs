using DocumentFormat.OpenXml.Math;
using HD.Fiscal.AccesoDatos;
using HD.Fiscal.Modelos;
using HD.Notifications.NotificacionesApp;
using HD.Security;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Fiscal
{
    public class ListadoInvoiceMoviemientosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public ListadoInvoiceMoviemientosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerListadoInvoiceMovimientos(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerListados(ejercicio, periodo);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtenerxml(string documento)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerXML(documento);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerListadosCorreccionIncidencias(int ejercicio, int periodo, string origen)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerCorreccionIncidencias(ejercicio, periodo, origen, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerListadosCorreccionIncidenciasAnticipos(int ejercicio, int periodo, string origen)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerCorreccionIncidenciasAnticipos(ejercicio, periodo, origen, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerListadoPosiblesAnticipos(string v_ref, string serie_fiscal, string importe)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Incidencias_Anticipos datos = new AD_Listado_Incidencias_Anticipos(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.obtenerAnticipos(v_ref, serie_fiscal, importe);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerListadoPosiblesCancelaciones(string v_ref, string serie_fiscal, string importe)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Incidencias_Anticipos datos = new AD_Listado_Incidencias_Anticipos(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.obtenerCancelaciones(v_ref, serie_fiscal, importe);
            return Ok(result);
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerListadoInvoice(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.obtenerInvoice(ejercicio, periodo);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerListadoMovimientosContables(int batch, int invoice)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.obtenerMovimientosContables(batch, invoice);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerDetalleCandidatos(int document_no, string von_no)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.obtenerDetalleCandidatos(document_no, von_no);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarDocumentoInvoice(int documento, string serie, int folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.buscarDocumento(documento, serie, folio);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarRelacion(mdl_Guardar_Relacion_InvoiceMovimiento mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            //mdl.usuario = int.Parse(Sesion.usuario());
            await datos.GuardarRelacion(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarRelacionNotaAnticipo(mdl_Relacion_Nota_Anticipo mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.GuardarRelacionNotaAnticipo(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarRelacionAnticipoCancelacion(mdl_Relacion_Anticipo_Cancelacion mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.GuardarRelacionAnticipoCancelacion(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AplicarReversa(mdl_Aplicar_Reversa mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            //mdl.usuario = int.Parse(Sesion.usuario());
            await datos.AplicarReversa(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AplicarRefacturacion(mdl_Aplicar_Refacturacion_Documento mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            //mdl.usuario = int.Parse(Sesion.usuario());
            await datos.AplicarRefacturacion(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }
    }
}
