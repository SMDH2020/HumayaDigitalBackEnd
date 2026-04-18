using HD.Security;
using HD_Finanzas.AccesoDatos.RotacionCXC;
using HD_Finanzas.AccesoDatos.RotacionInventario;
using HD_Finanzas.Modelos.RotacionCXC;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> ReporteCXC(int ejercicio, int periodo, string adr, string sucursales, string departamentos, string tipoReporte)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_RotacionCXC_Reporte datos = new AD_RotacionCXC_Reporte(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.reporte(ejercicio, periodo, adr, sucursales, departamentos, usuario, tipoReporte);
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
        public async Task<ActionResult> ListadoGuiaCXC(int ejercicio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            GuiaCXCListado datos = new GuiaCXCListado(CadenaConexion);
            var result = await datos.Listado(ejercicio);
            return Ok(result);
        }
    }
}
