using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.OrdenesAbiertas;

namespace HD.Endpoints.Controllers.Ventas
{
    public class OrdenTrabajoAbiertaSucursalController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public OrdenTrabajoAbiertaSucursalController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerOrdenesAbiertasSucursal(int sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_OrdenTrabajoAbiertaSucursal datos = new AD_OrdenTrabajoAbiertaSucursal(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            
            var result = await datos.ObtenerOrdenesAbiertasSucursal(usuario, sucursal);
            return Ok(result);
        }
    }
}