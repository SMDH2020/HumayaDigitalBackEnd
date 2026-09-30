using HD.Clientes.Consultas.Credito;
using HD.Clientes.Modelos.Credito;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HD.Endpoints.Controllers.Credito.DiferenciaVencimiento
{
    public class DetalleFinanciamientoPedidoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public DetalleFinanciamientoPedidoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Detalle(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Financiamiento_Pedido datos = new AD_Detalle_Financiamiento_Pedido(CadenaConexion);
            var result = await datos.detalle(folio);
            return Ok(result);
        }
    }
}
