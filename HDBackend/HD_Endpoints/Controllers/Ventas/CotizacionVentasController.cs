using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Ventas.Consultas.CotizacionesVentas;

namespace HD.Endpoints.Controllers.Ventas
{
    public class CotizacionVentasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CotizacionVentasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ConsultarFolio(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_CotizacionVentas datos = new AD_CotizacionVentas(CadenaConexion);
            string usuario = Sesion.usuario();
            var result = await datos.ObtenerByFolio(usuario, folio);
            return Ok(result);
        }

    }
}
