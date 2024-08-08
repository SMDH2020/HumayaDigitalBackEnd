using HD_Dashboard.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Dashboard
{
    public class ListadoVendedoresDashController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ListadoVendedoresDashController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> Obtener_Vendedores()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Vendedores_Dash datos = new AD_Listado_Vendedores_Dash(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ListadoVendedores(usuario);
            return Ok(result);
        }
    }
}
