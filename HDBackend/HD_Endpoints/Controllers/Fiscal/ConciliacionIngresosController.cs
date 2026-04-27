using HD.Fiscal.AccesoDatos;
using HD.Fiscal.Modelos;
using HD.Fiscal.Reportes;
using HD.Security;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using Microsoft.AspNetCore.Mvc;
using Ventas.Consultas.CotizacionesVentas;
using Ventas.Modelos.CotizacionesVentas;
using Ventas.Reportes;

namespace HD.Endpoints.Controllers.Fiscal
{
    public class ConciliacionIngresosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public ConciliacionIngresosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ConciliacionIngresosInvoice(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Conciliacion_Ingresos datos = new AD_Conciliacion_Ingresos(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.obtenerInvoice(ejercicio, periodo);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ConciliacionIngresosAnalitica(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Conciliacion_Ingresos datos = new AD_Conciliacion_Ingresos(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.obtenerAnalitica(ejercicio, periodo, usuario);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AplicarConciliacion(mdl_Conciliacion_Aplicar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Conciliacion_Ingresos datos = new AD_Conciliacion_Ingresos(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.AplicarConciliacion(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ActualizarConciliacion(mdl_Conciliacion_Actualizar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Conciliacion_Ingresos datos = new AD_Conciliacion_Ingresos(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            await datos.ActualizarConciliacion(mdl);

            return Ok(new
            {
                mensaje = "Guardado Correctamente",
            }
            );
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteAnalitica(int ejercicio, int periodo, string titulo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Conciliacion_Ingresos datos = new AD_Conciliacion_Ingresos(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.obtenerAnalitica(ejercicio, periodo, usuario);
            var docresult = await XLS_Conciliacion_Ingresos_Analitica.GenerarExcel(result.Analitica, titulo);
            return Ok(docresult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirExcelReporteInvoice(int ejercicio, int periodo, string titulo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Conciliacion_Ingresos datos = new AD_Conciliacion_Ingresos(CadenaConexion);
            var result = await datos.obtenerInvoice(ejercicio, periodo);
            var docresult = await XLS_ConciliacionIngresos_Invoice.GenerarExcel(result, titulo);
            return Ok(docresult);
        }
    }
}
