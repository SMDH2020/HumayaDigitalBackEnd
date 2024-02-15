using HD.Clientes.Consultas.PedidoCondicionesCredito;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class PedidoCondicionesCreditoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PedidoCondicionesCreditoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlPedido_Condiciones_Venta mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_PedidoCondicionesVenta_Guardar datos = new AD_PedidoCondicionesVenta_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            //AD_ClientesDatosPersonaFisica_Guardar datosfisica = new AD_ClientesDatosPersonaFisica_Guardar(CadenaConexion);
            //await datosfisica.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_PedidoCondicionesVenta_Listado datos = new AD_PedidoCondicionesVenta_Listado(CadenaConexion);
            var result = await datos.Get(folio);
            return Ok(result);

        }
    }
}
