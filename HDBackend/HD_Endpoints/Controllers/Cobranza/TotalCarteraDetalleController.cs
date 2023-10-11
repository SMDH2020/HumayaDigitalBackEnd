using HD.Security;
using HD_Cobranza.Capturas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class TotalCarteraDetalleController :MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public TotalCarteraDetalleController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idsucursal,string linea)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_TotalCarteraDetalle datos = new ADCob_TotalCarteraDetalle(CadenaConexion);
            var result = await datos.Listado(idsucursal,linea);
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoPorCliente(int cliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_TotalCarteraDetalle datos = new ADCob_TotalCarteraDetalle(CadenaConexion);
            var result = await datos.ListadoPorCliente(cliente);
            return Ok(result);

        }
    }
}
