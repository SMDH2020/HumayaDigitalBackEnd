using HD.Clientes.Consultas.CRM.Proceso6Contactos;
using HD.Clientes.Modelos.CRM.Proceso6Contactos;
using HD.Clientes.Reportes;
using HD.Notifications;
using HD.Notifications.CRM;
using HD.Notifications.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.CRM
{
    public class Proceso6ContactosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public Proceso6ContactosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoProceso6Contactos(
    string fecha_inicio, string fecha_fin, string adr, string sucursal, int usuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Proceso_6_Contactos datos = new AD_Proceso_6_Contactos(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(fecha_inicio, fecha_fin, adr, sucursal, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerTimelineEquipoEntregado(int id_CEE)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Proceso_6_Contactos datos = new AD_Proceso_6_Contactos(CadenaConexion);
            var result = await datos.ObtenerTimeline(id_CEE);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> IndicarContactoRealizado(mdl_Indicar_Realizado mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Proceso_6_Contactos datos = new AD_Proceso_6_Contactos(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.IndicarRealizado(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteProceso6Contactos(
            string fecha_inicio, string fecha_fin, string adr, string sucursal,
            string fase, int responsable, int usuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Proceso_6_Contactos datos = new AD_Proceso_6_Contactos(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.Reporte(fecha_inicio, fecha_fin, adr, sucursal,
                                             fase, responsable, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> FiltroResponsablesC6C(string fecha_inicio, string fecha_fin)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Proceso_6_Contactos datos = new AD_Proceso_6_Contactos(CadenaConexion);
            var result = await datos.FiltroResponsables(fecha_inicio, fecha_fin);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ExcelReporteProceso6Contactos(
            string fecha_inicio, string fecha_fin, string adr, string sucursal,
            string fase, int responsable, int usuario, string? titulo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Proceso_6_Contactos datos = new AD_Proceso_6_Contactos(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.Reporte(fecha_inicio, fecha_fin, adr, sucursal,
                                             fase, responsable, usuario);
            var doc = await XLSVen_Proceso_6Contactos_Reporte.GenerarExcel(result, titulo);
            return Ok(doc);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ExcelListadoProceso6Contactos(
            string fecha_inicio, string fecha_fin, string adr, string sucursal,
            int usuario, string? titulo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Proceso_6_Contactos datos = new AD_Proceso_6_Contactos(CadenaConexion);
            usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(fecha_inicio, fecha_fin, adr, sucursal, usuario);
            var doc = await XLSVen_Proceso_6Contactos_Listado.GenerarExcel(result, titulo);
            return Ok(doc);
        }

        
    }
}
