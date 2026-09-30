using HD.Clientes.Consultas.Facturar_Equipo;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Ventas.Consultas.FacturasPendientes;

namespace HD.Endpoints.Controllers.Ventas
{
    public class VentasFacturacionPendienteController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public VentasFacturacionPendienteController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener_Facturacion_Pendiente()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Facturas_Pendientes_Obtener_Listado datos = new AD_Facturas_Pendientes_Obtener_Listado(CadenaConexion);
            var usuario = Sesion.usuario();
            //int.Parse(Sesion.usuario());
            var result = await datos.Listado(usuario);
            return Ok(result);

        }
    }
}
