using HD.Security;
using HD_Ventas.Consultas.Inventario;
using HD_Ventas.Modelos.Inventario;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class InventarioModelosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public InventarioModelosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Inventario_Modelos_Listado datos = new AD_Inventario_Modelos_Listado(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_Inventario_Modelos_Listado mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Inventario_Modelos_Guardar datos = new AD_Inventario_Modelos_Guardar(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            await datos.Guardar(mdl, usuario);
            return Ok(new { mensaje = "Guardado Correctamente" });
        }
    }
}
