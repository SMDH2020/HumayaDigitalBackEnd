using HD.Security;
using HD_Dashboard.Consultas;
using HD_Dashboard.Consultas.Vendedor;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Dashboard
{
    public class DashClientesFacturacionDetalleController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public DashClientesFacturacionDetalleController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Detalle(int idcliente,string linea)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            Dash_Clientes_Facturacion_Detalle datos = new Dash_Clientes_Facturacion_Detalle(CadenaConexion);
            var result = await datos.Detalle(idcliente,linea);
            return Ok(result);

        }
    }
}
