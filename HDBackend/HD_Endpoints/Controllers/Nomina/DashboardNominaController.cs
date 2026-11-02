using HD.Security;
using HD_Nomina.Consultas;
using HD_Nomina.Reportes;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Nomina
{
        public class DashboardNominaController : MyBase
        {
            private readonly IConfiguration Configuracion;
            private readonly ISesion Sesion;
            public DashboardNominaController(IConfiguration configuration, ISesion sesion)
            {
                Configuracion = configuration;
                Sesion = sesion;
            }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerDashboard(int? periodo_inicio = null, int? periodo_fin = null,
                                             int? ejercicio_inicio = null, int? ejercicio_fin = null,
                                             string adr = "0", string sucursales = "0", string departamentos = "0")
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard datos = new AD_Dashboard(CadenaConexion);
            var result = await datos.obtenerDashboard(periodo_inicio, periodo_fin, ejercicio_inicio, ejercicio_fin,
                                                      adr, sucursales, departamentos);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcelListadoEmpleados(int? periodo_inicio = null, int? periodo_fin = null,
                                                                     int? ejercicio_inicio = null, int? ejercicio_fin = null,
                                                                     string adr = "0", string sucursales = "0", string departamentos = "0")
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard datos = new AD_Dashboard(CadenaConexion);
            var result = await datos.obtenerDashboard(periodo_inicio, periodo_fin, ejercicio_inicio, ejercicio_fin,
                                                      adr, sucursales, departamentos);
            var docresult = await XLS_Listado_Empleados.GenerarExcel(result.listado_empleado);
            return Ok(docresult);
        }
    }
    
}
