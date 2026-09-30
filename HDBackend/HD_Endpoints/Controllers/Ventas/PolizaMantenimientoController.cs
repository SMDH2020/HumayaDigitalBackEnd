using HD.Security;
using HD_Ventas.Consultas.PaqueteServicios;
using HD_Ventas.Modelos.PaqueteServicios;
using HD_Ventas.Reportes;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class PolizaMantenimientoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PolizaMantenimientoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Guardar(mdl_Poliza_Mantenimiento_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Poliza_Mantenimiento_Guardar datos = new AD_Poliza_Mantenimiento_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente"
            });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int ejercicioInicio, int periodoInicio, int ejercicioFin, int periodoFin, string? region, string? sucursal, string? vendedor)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Poliza_Mantenimiento_Listado datos = new AD_Poliza_Mantenimiento_Listado(CadenaConexion);
            var result = await datos.Listado(ejercicioInicio, periodoInicio, ejercicioFin, periodoFin, region, sucursal, vendedor,Sesion.usuario());
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerID(int idpoliza)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Poliza_Mantenimiento_ObtenerID datos = new AD_Poliza_Mantenimiento_ObtenerID(CadenaConexion);
            var result = await datos.Listado(idpoliza);
            return Ok(result);

        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Eliminar(int idpoliza)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Poliza_Mantenimiento_Eliminar datos = new AD_Poliza_Mantenimiento_Eliminar(CadenaConexion);
            await datos.eliminar(idpoliza);
            return Ok(new
            {
                mensaje = "Eliminado Correctamente"
            });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcel(int ejercicioInicio, int periodoInicio, int ejercicioFin, int periodoFin, string? region, string? sucursal, string? vendedor)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Poliza_Mantenimiento_Listado datos = new AD_Poliza_Mantenimiento_Listado(CadenaConexion);
            string usuario = vendedor;
            int sesion = int.Parse(Sesion.usuario());
            var result = await datos.Listado(ejercicioInicio, periodoInicio, ejercicioFin, periodoFin, region, sucursal, vendedor, Sesion.usuario());
            var docresult = await XLSVen_Poliza_Mantenimiento_Excel.GenerarExcel(result, ejercicioFin, periodoFin, ejercicioInicio, periodoInicio);
            return Ok(docresult);
        }
    }
}
