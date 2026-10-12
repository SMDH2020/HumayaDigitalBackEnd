using HD_Cobranza.Capturas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Cobranza.Capturas.PlantillaKarbot;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class PedidosFacturadosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PedidosFacturadosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> CargaPedidosFacturados(int ejercicio, int periodo, string sucursales, string adr, string operacion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCarga_Pedidos_Facturados datos = new ADCarga_Pedidos_Facturados(CadenaConexion);
            //int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Pedidos(ejercicio, periodo, adr, sucursales, operacion);
            return Ok(result);
        }
    }
}
