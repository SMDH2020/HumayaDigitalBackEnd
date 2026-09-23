using HD.Security;
using HD_Cobranza.Capturas.ReportePrestamosClientes;
using HD_Cobranza.Reportes;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class ReportePrestamosClientesController : MyBase
    {
        private static readonly string[] OrigenesValidos =
        {
            "FACTURACION CON SOLICITUD",
            "FACTURACION SIN SOLICITUD",
            "SOLICITUD SIN FACTURACION"
        };

        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ReportePrestamosClientesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public ActionResult Origenes()
        {
            return Ok(OrigenesValidos);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(string? origen_registro, DateTime? fecha_desde, DateTime? fecha_hasta, string? adr, string? sucursal)
        {
            var validacion = Validar(ref origen_registro, fecha_desde, fecha_hasta);
            if (validacion != null) return validacion;

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ReportePrestamosClientes datos = new AD_ReportePrestamosClientes(CadenaConexion);
            var result = await datos.Listado(origen_registro, fecha_desde, fecha_hasta, adr, sucursal);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(string? origen_registro, DateTime? fecha_desde, DateTime? fecha_hasta, string? adr, string? sucursal)
        {
            var validacion = Validar(ref origen_registro, fecha_desde, fecha_hasta);
            if (validacion != null) return validacion;

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ReportePrestamosClientes datos = new AD_ReportePrestamosClientes(CadenaConexion);
            var result = await datos.Listado(origen_registro, fecha_desde, fecha_hasta, adr, sucursal);
            var docResult = await XLSCob_ReportePrestamosClientes.CrearExcel(result);
            return Ok(docResult);
        }

        private ActionResult? Validar(ref string? origen_registro, DateTime? fecha_desde, DateTime? fecha_hasta)
        {
            if (!string.IsNullOrWhiteSpace(origen_registro))
            {
                string origen = origen_registro.Trim().ToUpper();
                if (!OrigenesValidos.Contains(origen))
                    return BadRequest(new { mensaje = "El valor de origen_registro no es valido", recibido = origen_registro, valores_validos = OrigenesValidos });

                origen_registro = origen;
            }

            if (fecha_desde.HasValue && fecha_hasta.HasValue && fecha_desde.Value.Date > fecha_hasta.Value.Date)
                return BadRequest(new { mensaje = "La fecha_desde no puede ser posterior a la fecha_hasta" });

            return null;
        }
    }
}
