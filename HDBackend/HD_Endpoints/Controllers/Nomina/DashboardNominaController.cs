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
        public async Task<ActionResult> ObtenerDashboard(int ejercicio, string adr, string sucursales, string departamentos, string header)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard datos = new AD_Dashboard(CadenaConexion);
            var result = await datos.obtenerDashboard(ejercicio, adr, sucursales, departamentos, header);
            return Ok(result);
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetFotoEmpleado(int idempleado)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard datos = new AD_Dashboard(CadenaConexion);
            var result = await datos.GetFotoEmpleado(idempleado);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcelListadoEmpleados(int ejercicio, string adr, string sucursales, string departamentos, string header)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Dashboard datos = new AD_Dashboard(CadenaConexion);
            var result = await datos.obtenerDashboard(ejercicio, adr, sucursales, departamentos, header);
            var docresult = await XLS_Listado_Empleados.GenerarExcel(result.listado_empleado);
            return Ok(docresult);
        }
    }
    
}
