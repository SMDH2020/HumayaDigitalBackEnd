using HD.Clientes.Consultas.PedidoDatosGenerales;
using HD.Clientes.Modelos;
using HD.Notifications.NotificacionesApp;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class PedidoDatosGeneralesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PedidoDatosGeneralesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlPedido_Datos_Generales mdl)
        {
            if (mdl is null)
            {
                return BadRequest("Error en datos enviados");
            }
            if (ModelState.IsValid)
            {
                string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
                AD_PedidosGenerales_Guardar datos = new AD_PedidosGenerales_Guardar(CadenaConexion);
                mdl.usuario = Sesion.usuario();
                await datos.Guardar(mdl);
                return Ok(new { mensaje = "datos cargados con exito" });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetByFolio(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_PedidosGenerales_GetByFolio datos = new AD_PedidosGenerales_GetByFolio(CadenaConexion);
            var result = await datos.Get(folio);

            string origen = Sesion.origen();
            if (Sesion.generarLog() == true && origen == "APP")
            {
                NE_Logs_App_HD log = new NE_Logs_App_HD(CadenaConexion);
                await log.Guardar($"Reviso informacion de pedido con folio: {folio}", origen, Sesion.usuario());
            }

            return Ok(result);

        }
    }
}
