using HD.Security;
using HD_Ventas.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class DetalleFacturacionMaquinariaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public DetalleFacturacionMaquinariaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> nip(string nip)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Detalle_Facturacion_Maquinaria datos = new AD_Detalle_Facturacion_Maquinaria(CadenaConexion);
            var result = await datos.Get(nip);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> idEliminar(int id, string usuario)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Eliminar_Facturacion_Maquinaria datos = new AD_Eliminar_Facturacion_Maquinaria(CadenaConexion);
            usuario = Sesion.usuario();
            var result = await datos.Eliminar(id, usuario);
            return Ok(result);
        }
    }
}
