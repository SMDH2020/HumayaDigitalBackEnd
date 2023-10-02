using HD.Clientes.Consultas.Clientes;
using HD.Security;
using HD_Dashboard.Consultas.Vendedor;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Dashboard
{
    public class DashVendedorController: MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public DashVendedorController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            Dash_Vendedor_Main datos = new Dash_Vendedor_Main(CadenaConexion);
            var result = await datos.Dashboard();
            return Ok(result);

        }
    }
}
