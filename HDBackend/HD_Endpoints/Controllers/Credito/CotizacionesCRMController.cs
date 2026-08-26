using HD.Clientes.Consultas.CRM.Cotizaciones;
using HD.Clientes.Consultas.CRM.Reportes;
using HD.Clientes.Modelos.CRM.Cotizaciones;
using HD.Clientes.Reportes;
using HD.Security;
using HD.Clientes.Consultas.SolicitudCreditoDocumento;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class CotizacionesCRMController : MyBase
    {

        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CotizacionesCRMController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Catalogo_ServicioCRM datos = new AD_Catalogo_ServicioCRM(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_Catalogo_ServicioCRM mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Catalogo_ServicioCRM datos = new AD_Catalogo_ServicioCRM(CadenaConexion);
            mdl.Usuario = int.Parse(Sesion.usuario());
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CambiarEstatus(int idServicio, bool estatus)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Catalogo_ServicioCRM datos = new AD_Catalogo_ServicioCRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.CambiarEstatus(idServicio, estatus, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerServicioID(int idServicio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Catalogo_ServicioCRM datos = new AD_Catalogo_ServicioCRM(CadenaConexion);
            var result = await datos.ObtenerServicioID(idServicio);
            return Ok(result);
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoCotizaciones(string fechainicio, string fechafin, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cotizaciones_CRM datos = new AD_Cotizaciones_CRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(fechainicio, fechafin, adr, sucursal, usuario);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarCotizaciones(mdl_Cotizaciones_CRM_Guarad mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cotizaciones_CRM datos = new AD_Cotizaciones_CRM(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Guardar(mdl);
            return Ok(new { folio = result });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EliminarCotizaciones(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cotizaciones_CRM datos = new AD_Cotizaciones_CRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Eliminar(folio, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerCotizacionPorFolio(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cotizaciones_CRM datos = new AD_Cotizaciones_CRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerPorFolio(folio, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDF(string folio, string plantilla)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cotizaciones_CRM datos = new AD_Cotizaciones_CRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());

            var vista = await datos.ObtenerPorFolio(folio, usuario);

            try
            {
                if (vista?.Cotizacion == null)
                    return BadRequest("No se encontró la cotización.");

                RPT_Result documento = RPT_Cotizacion_CRM.GenerarPDF(vista, plantilla);
                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");
            }
        }
    }
}
