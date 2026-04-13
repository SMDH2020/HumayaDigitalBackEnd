using HD.Fiscal.AccesoDatos;
using HD.Fiscal.Modelos;
using HD.Security;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;
using Microsoft.AspNetCore.Mvc;

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
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.obtenerAnalitica(ejercicio, periodo);
            return Ok(result);
        }
    }
}
