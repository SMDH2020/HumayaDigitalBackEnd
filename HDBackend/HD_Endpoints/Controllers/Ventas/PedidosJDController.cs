using HD.Endpoints.Filtros;
using HD.Security;
using HD_Ventas.Consultas.PedidosJD;
using HD_Ventas.Modelos.PedidosJD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    /// <summary>
    /// Integracion de sistema a sistema: se autentica con el header X-Api-Key
    /// (clave en configuracion "PedidosJD:ApiKey"), no con JWT.
    /// </summary>
    [AllowAnonymous]
    [ApiKey("PedidosJD:ApiKey")]
    public class PedidosJDController : MyBase
    {
        // Usuario de servicio con el que se registran los pedidos (no hay sesion JWT)
        private const int UsuarioServicio = 5630;

        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PedidosJDController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Lista(mdl_PedidosJD_Lista_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_PedidosJD_Lista_Guardar datos = new AD_PedidosJD_Lista_Guardar(CadenaConexion);
            var result = await datos.Guardar(mdl, UsuarioServicio);
            return Ok(new { ok = true, recibidos = result.recibidos, nuevos = result.nuevos, actualizados = result.actualizados, pendientes_detalle = result.pendientes_detalle });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Detalle(mdl_PedidosJD_Detalle_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_PedidosJD_Detalle_Guardar datos = new AD_PedidosJD_Detalle_Guardar(CadenaConexion);
            var result = await datos.Guardar(mdl);
            return Ok(new { ok = true, pedidos = result.pedidos, lineas = result.lineas, comentarios = result.comentarios });
        }
    }
}
