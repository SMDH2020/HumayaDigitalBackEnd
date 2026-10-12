using HD.Clientes.Consultas.CRM.Reportes;
using HD.Clientes.Consultas.CRM.Visitas;
using HD.Security;
using HD_Reporteria.CRM;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ReportesCRMController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ReportesCRMController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteVisitasProgramadas(string fechainicio, string fechafin, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Visitas_ProgramadasCRM datos = new AD_Reporte_Visitas_ProgramadasCRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ReporteVisitasProgramadas(fechainicio, fechafin, adr, sucursal, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteInformacionCapturada(string verificado, string etiqueta, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Informacion_CapturadaCRM datos = new AD_Reporte_Informacion_CapturadaCRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(verificado, etiqueta, adr, sucursal, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteGeolocalizacion(string geolocalizacion, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_GeolocalizacionCRM datos = new AD_Reporte_GeolocalizacionCRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(geolocalizacion, adr, sucursal, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteCoberturaVisitas(string fechainicio, string fechafin, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Cobertura_VisitasCRM datos = new AD_Reporte_Cobertura_VisitasCRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(fechainicio, fechafin, adr, sucursal, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteCoberturaCartera(string fechainicio, string fechafin, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Cobertura_CarteraCRM datos = new AD_Reporte_Cobertura_CarteraCRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(fechainicio, fechafin, adr, sucursal, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteVisitasProgramadas(string fechainicio, string fechafin, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Visitas_ProgramadasCRM datos = new AD_Reporte_Visitas_ProgramadasCRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ReporteVisitasProgramadas(fechainicio, fechafin, adr, sucursal, usuario);
            var docresult = await XLS_Reporte_Visitas.GenerarExcel(result.listado_visitas);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteInformacionCapturada(string verificado, string etiqueta, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Informacion_CapturadaCRM datos = new AD_Reporte_Informacion_CapturadaCRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(verificado, etiqueta, adr, sucursal, usuario);
            var docresult = await XLS_Reporte_Informacion_Capturada.GenerarExcel(result.listado);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteGeolocalizacion(string geolocalizacion, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_GeolocalizacionCRM datos = new AD_Reporte_GeolocalizacionCRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(geolocalizacion, adr, sucursal, usuario);
            var docresult = await XLS_Reporte_Geolocalizacion.GenerarExcel(result.listado);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteCoberturaVisitas(string fechainicio, string fechafin, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Cobertura_VisitasCRM datos = new AD_Reporte_Cobertura_VisitasCRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(fechainicio, fechafin, adr, sucursal, usuario);
            var docresult = await XLS_Reporte_CoberturaVisitas.GenerarExcel(result.listado);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteCoberturaCartera(string fechainicio, string fechafin, string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Cobertura_CarteraCRM datos = new AD_Reporte_Cobertura_CarteraCRM(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(fechainicio, fechafin, adr, sucursal, usuario);
            var docresult = await XLS_Reporte_CoberturaCartera.GenerarExcel(result.listado);
            return Ok(docresult);
        }

    }
}
