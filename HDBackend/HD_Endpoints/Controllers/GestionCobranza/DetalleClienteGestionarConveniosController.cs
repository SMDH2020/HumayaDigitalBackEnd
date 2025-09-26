using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.GestionCobranza.Modelos;
using HD_Cobranza.Reportes;
using HD_Reporteria;
using HD_Reporteria.Cobranza;
using HD_Reporteria.GestionCobranza;
using HD_Reporteria.Ventas;
using HD_Ventas.Consultas;
using HD_Ventas.Reportes;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionCobranza
{
    public class DetalleClienteGestionarConveniosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public DetalleClienteGestionarConveniosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Cliente_Gestionar_Convenios datos = new AD_Detalle_Cliente_Gestionar_Convenios(CadenaConexion);
            var result = await datos.Get();
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoParametros(string? fechainicio, string? fechafin, string adr, string sucursal, int responsable)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Cliente_Gestionar_Convenios_Parametros datos = new AD_Detalle_Cliente_Gestionar_Convenios_Parametros(CadenaConexion);
            var result = await datos.Get(fechainicio, fechafin, adr, sucursal, responsable);
            return Ok(result);
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporte(string? fechainicio, string? fechafin, string adr, string sucursal, int responsable, string? titulo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Cliente_Gestionar_Convenios_Parametros datos = new AD_Detalle_Cliente_Gestionar_Convenios_Parametros(CadenaConexion);
            var result = await datos.Get(fechainicio, fechafin, adr, sucursal, responsable);
            var docresult = await XLSCob_Listado_Convenios_Realizados.GenerarExcel(result, titulo);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDFReporte(string? fechainicio, string? fechafin, string adr, string sucursal, int responsable, string? titulo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Cliente_Gestionar_Convenios_Parametros datos = new AD_Detalle_Cliente_Gestionar_Convenios_Parametros(CadenaConexion);
            var result = await datos.Get(fechainicio, fechafin, adr, sucursal, responsable);

            try
            {
                RPT_Result documento = RPT_Listado_Convenios_Realizados.GenerarPDF(result, titulo);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CargarEvidencia(mdl_Convenio_Guardar mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Evidencia_Modal datos = new AD_Carga_Evidencia_Modal(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito", listado = result });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReportePDF(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Convenio_Impresion datos = new AD_Convenio_Impresion(CadenaConexion);
            var result = await datos.Get(folio);
            try
            {
                RPT_Result documento = RPT_Convenio_Guardado.GenerarPDF(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
