using HD.Security;
using HD_Ventas.Consultas.InventarioModelo;
using HD_Ventas.Modelos.InventarioModelo;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class InventarioModeloController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public InventarioModeloController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_InventarioModelo_Listado datos = new AD_InventarioModelo_Listado(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Excluir(mdl_InventarioModelo_Excluir mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_InventarioModelo_Excluir datos = new AD_InventarioModelo_Excluir(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            await datos.Excluir(mdl, usuario);
            return Ok(new { mensaje = "Guardado Correctamente" });
        }
    }
}
