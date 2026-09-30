
using HD.Clientes.Consultas.Credito;
using HD.Clientes.Modelos.Credito;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HD.Endpoints.Controllers.Credito.DiferenciaVencimiento
{
    public class FacturasDiferenciaVencimientoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FacturasDiferenciaVencimientoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Facturas()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Facturas_Diferencias_Vencimiento datos = new AD_Facturas_Diferencias_Vencimiento(CadenaConexion);
            var result = await datos.facturas();
            return Ok(result);
        }
    }
}
