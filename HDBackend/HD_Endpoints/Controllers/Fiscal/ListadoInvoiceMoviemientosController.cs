using HD.Fiscal.AccesoDatos;
using HD.Fiscal.Modelos;
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
        public async Task<ActionResult> ObtenerListadosCorreccionIncidencias(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerCorreccionIncidencias(ejercicio, periodo, usuario);
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
        public async Task<ActionResult> BuscarDocumentoInvoice(int documento)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_InvoiceMoviemientos datos = new AD_Listado_InvoiceMoviemientos(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.buscarDocumento(documento);
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
