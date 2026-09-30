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
        public async Task<ActionResult> ObtenerGeneral(int ejercicio, string sucursales, string adr,string periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Reporte_Cumplimiento_Compromiso_Condicionado datos = new AD_Reporte_Cumplimiento_Compromiso_Condicionado(CadenaConexion);
            var result = await datos.Obtener(ejercicio, sucursales,adr,periodo);
            result.resumen.titulo = $"OPERACIONES CONDICIONADAS - {ejercicio}";
            result.resumen.ejercicio = ejercicio;
            //result.resumen.sucursal = sucursal;
            var ArrayMes = result.detalle.GroupBy(item => item.mes).Select(item => item.Key).ToList();
            var totalesPorMes = result.detalle
                .GroupBy(item => item.mes).Select(group => new
                {
                    mes = group.Key,
                    total_solicitados = group.Sum(item => item.documentos_solicitados),
                    total_documentos_entregados = group.Sum(item => item.documentos_entregados),
                    total_porcentaje_documentos_entregados = Math.Round(group.Sum(item => item.porcentaje_documentos_entregados) / (double)group.Count(), 2),
                    total_documentos_faltantes = group.Sum(item => item.documentos_faltantes),
                    total_porcentaje_documentos_faltantes = Math.Round(group.Sum(item => item.porcentaje_documentos_faltantes) / (double)group.Count(), 2),
                    total_documentos_puntuales = group.Sum(item => item.documentos_puntuales),
                    total_porcentaje_documentos_puntuales = Math.Round(group.Sum(item => item.porcentaje_documentos_puntuales) / (double)group.Count(), 2),
                    total_documentos_retrasados = group.Sum(item => item.documentos_retrasados),
                    total_porcentaje_documentos_retrasados = Math.Round(group.Sum(item => item.porcentaje_documentos_retrasados) / (double)group.Count(), 2),
                    total_dias_vencido = group.Where(item => item.dias_vencido >= 0).Sum(item => item.dias_vencido),
                })
                .ToList();
            return Ok(new { ArrayMes, result, TotalesPorMes = totalesPorMes });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerDetalle(int usuario, int ejercicio, string sucursales, string adr, string periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cumplimiento_Compromiso_Condicionado_Detalle datos = new AD_Cumplimiento_Compromiso_Condicionado_Detalle(CadenaConexion);
            var result = await datos.Obtenerdetalle(usuario, ejercicio, sucursales, adr, periodo);
            var ArrayMes = result.GroupBy(item => item.mes).Select(item => item.Key ).ToList();
            // Calcular los totales por mes
            var totalesPorMes = result
                .GroupBy(item => item.mes).Select(group => new
                {
                    mes = group.Key,
                    total_solicitados = group.Sum(item => item.documentos_solicitados),
                    total_documentos_entregados = group.Sum(item => item.documentos_entregados),
                    total_porcentaje_documentos_entregados = Math.Round(group.Sum(item => item.porcentaje_documentos_entregados) / (double)group.Count(), 2),
                    total_documentos_faltantes = group.Sum(item => item.documentos_faltantes),
                    total_porcentaje_documentos_faltantes = Math.Round(group.Sum(item => item.porcentaje_documentos_faltantes) / (double)group.Count(), 2),
                    total_documentos_puntuales = group.Sum(item => item.documentos_puntuales),
                    total_porcentaje_documentos_puntuales = Math.Round(group.Sum(item => item.porcentaje_documentos_puntuales) / (double)group.Count(), 2),
                    total_documentos_retrasados = group.Sum(item => item.documentos_retrasados),
                    total_porcentaje_documentos_retrasados = Math.Round(group.Sum(item => item.porcentaje_documentos_retrasados) / (double)group.Count(), 2),
                    total_dias_vencido = group.Where(item => item.dias_vencido >= 0).Sum(item => item.dias_vencido),
                })
                .ToList();
            return Ok(new { ArrayMes, result, TotalesPorMes = totalesPorMes });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerDetalleFolio(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cumplimiento_Compromiso_Detalle_PorFolio datos = new AD_Cumplimiento_Compromiso_Detalle_PorFolio(CadenaConexion);
            var result = await datos.ObtenerdetalleFolio(folio);
            //var ArrayMes = result.GroupBy(item => item.mes).Select(item => item.Key).ToList();
            return Ok(result);

        }
    }
}
