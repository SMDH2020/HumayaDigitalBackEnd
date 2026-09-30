using HD_Ventas.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class ListadoVendedoresController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ListadoVendedoresController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> Obtener_Vendedores()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Vendedores datos = new AD_Listado_Vendedores(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Listado(usuario);
            return Ok(result);
        }
    }
}
