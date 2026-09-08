using HD_Ventas.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Ventas.Modelos;

namespace HD.Endpoints.Controllers.Ventas
{
    public class ListadoVendedoresFiltroController : ControllerBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ListadoVendedoresFiltroController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener_Vendedores_Filtro(int ejercicio, int idsucursalfiltro)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Vendedores_Filtro datos = new AD_Listado_Vendedores_Filtro(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ListadoFiltro(ejercicio, usuario, idsucursalfiltro);
            return Ok(result);
        }
    }
}