using HD_Buro.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Ventas.Consultas;
using HD_Cobranza.Capturas;
using HD_Cobranza.Reportes;
using HD_Reporteria.Buro_Credito;
using HD_Buro.Modelos;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace HD.Endpoints.Controllers.BuroCredito
{
    public class CargaReporteBuroController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public CargaReporteBuroController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Cargar(int ejercicio, int periodo, int sucursal, string mostrar)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Reporte_Buro datos = new AD_Carga_Reporte_Buro(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.reporte(ejercicio, periodo, sucursal, mostrar);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(int ejercicio, int periodo, int sucursal, string mostrar)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Reporte_Buro datos = new AD_Carga_Reporte_Buro(CadenaConexion);
            var result = await datos.reporte(ejercicio, periodo, sucursal, mostrar);
            var docResult = await XLS_Reporte_Buro.CrearExcel(result);
            return Ok(docResult);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcelInforme(mdlReporteBuroView view)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_REporteBuro_Credito datos = new AD_REporteBuro_Credito(CadenaConexion);
            var result = await datos.reporte(view.ejercicio, view.periodo);
            var docResult = await XLS_Reporte_Mensual_Buro.CrearExcel(result,view.mes,view.ejercicio);
            return Ok(docResult);
        }
    }
}
