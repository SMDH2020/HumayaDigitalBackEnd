using HD.Security;
using HD_Finanzas.AccesoDatos.RotacionCXC;
using HD_Finanzas.AccesoDatos.RotacionInventario;
using HD_Finanzas.Modelos.RotacionCXC;
using HD_Reporteria.Finanzas.Excel;
using Microsoft.AspNetCore.Mvc;
using Ventas.Consultas.CotizacionesVentas;
using Ventas.Reportes;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class RotacionCXCController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public RotacionCXCController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteCXC(int ejercicio, int periodo, string tipoUbi , string id, string tipoReporte)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RotacionCXC_Reporte datos = new AD_RotacionCXC_Reporte(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.reporte(ejercicio, periodo, tipoUbi, id, usuario, tipoReporte);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteSucursal(int ejercicio, int periodo, string adr, string sucursales, string departamentos, string tipoReporte)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RotacionCXC_Reporte datos = new AD_RotacionCXC_Reporte(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.reporteSucursal(ejercicio, periodo, adr, sucursales, departamentos, usuario, tipoReporte);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> detalleCXC(int ejercicio, int periodo, string adr, string sucursales, string departamentos, string tipoReporte)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RotacionCXC_Detalle datos = new AD_RotacionCXC_Detalle(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.DetalleCXC(ejercicio, periodo, adr, sucursales, departamentos, usuario, tipoReporte);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarGuiaCXC(mdl_GuiaCXC_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_GuiaCXC_Guardar datos = new AD_GuiaCXC_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoGuiaCXC(int ejercicio, string tipo_ubi, int ubicacion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            GuiaCXCListado datos = new GuiaCXCListado(CadenaConexion);
            var result = await datos.Listado(ejercicio, tipo_ubi, ubicacion);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporte(int ejercicio, int periodo, string tipoUbi, string id, string tipoReporte, string titulo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RotacionCXC_Reporte datos = new AD_RotacionCXC_Reporte(CadenaConexion);
            string usuario = Sesion.usuario();
            var result = await datos.reporte(ejercicio, periodo, tipoUbi, id, usuario, tipoReporte);
            var docresult = await XLS_Rotacion_CXC.GenerarExcel(result.rotacion, titulo);
            //var servicio = await Conexion_Servicio_Mensajeria.send("home", new { });
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteOptimo(int ejercicio, int periodo, string tipoUbi, string id, string tipoReporte, string titulo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RotacionCXC_Reporte datos = new AD_RotacionCXC_Reporte(CadenaConexion);
            string usuario = Sesion.usuario();
            var result = await datos.reporte(ejercicio, periodo, tipoUbi, id, usuario, tipoReporte);
            var docresult = await XLS_Rotacion_CXC_Optima.GenerarExcel(result.rotacion, titulo);
            //var servicio = await Conexion_Servicio_Mensajeria.send("home", new { });
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelDetalle(int ejercicio, int periodo, string adr, string sucursales, string departamentos, string tipoReporte, string titulo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RotacionCXC_Detalle datos = new AD_RotacionCXC_Detalle(CadenaConexion);
            string usuario = Sesion.usuario();
            var result = await datos.DetalleCXC(ejercicio, periodo, adr, sucursales, departamentos, usuario, tipoReporte);
            var docresult = await XLS_Detalle_CXC.GenerarExcel(result, titulo);
            //var servicio = await Conexion_Servicio_Mensajeria.send("home", new { });
            return Ok(docresult);
        }

    }
}
