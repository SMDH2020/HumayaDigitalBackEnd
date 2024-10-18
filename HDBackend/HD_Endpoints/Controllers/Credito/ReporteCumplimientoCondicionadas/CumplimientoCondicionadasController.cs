using DocumentFormat.OpenXml.Drawing.Charts;
using HD.Clientes.Consultas.ClientesDocumentacion;
using HD.Clientes.Consultas.ReporteCompromisoCondicionadas;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.Reporte_Cumplimiento_Condicionadas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito.ReporteCumplimientoCondicionadas
{
    public class CumplimientoCondicionadasController : MyBase
    {

        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CumplimientoCondicionadasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerGeneral(int ejercicio, int sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Cumplimiento_Compromiso_Condicionado datos = new AD_Reporte_Cumplimiento_Compromiso_Condicionado(CadenaConexion);
            var result = await datos.Obtener(ejercicio, sucursal);
            result.resumen.titulo = $"OPERACIONES CONDICIONADAS - {ejercicio}";
            result.resumen.ejercicio = ejercicio;
            result.resumen.sucursal = sucursal;
            var ArrayMes = result.detalle.GroupBy(item => item.mes).Select(item => item.Key).ToList();
            return Ok(new { ArrayMes, result });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerDetalle(int usuario, int ejercicio, int sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cumplimiento_Compromiso_Condicionado_Detalle datos = new AD_Cumplimiento_Compromiso_Condicionado_Detalle(CadenaConexion);
            var result = await datos.Obtenerdetalle(usuario, ejercicio, sucursal);
            var ArrayMes = result.GroupBy(item => item.mes).Select(item => item.Key ).ToList();
            return Ok(new{ArrayMes, result});

        }
    }
}
