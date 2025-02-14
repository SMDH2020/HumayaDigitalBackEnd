using HD.Clientes.Consultas.ClientesCultivo;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Usados.Modelos.Usados;

namespace HD.Endpoints.Controllers.Usados.Inventario
{
    public class InventarioController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public InventarioController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(string Modelo, int ejercicio, string HP, string Sucursal, string Promocion, string Estatus)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Inventario_Listado datos = new AD_Inventario_Listado(CadenaConexion);
            var result = await datos.Listado(Modelo, ejercicio, HP, Sucursal, Promocion, Estatus);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoFiltro()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Inventario_Listado datos = new AD_Inventario_Listado(CadenaConexion);
            var result = await datos.ListadoFiltro();
            return Ok(result);

        }
    }
}
