using HD.Security;
using HD.Clientes.Consultas.Credito;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class FacturasNoRegistradasEquipController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FacturasNoRegistradasEquipController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoFacturas()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Facturas_noRegistradas_Equip datos = new AD_Facturas_noRegistradas_Equip(CadenaConexion);
            var result = await datos.facturas();
            return Ok(result);
        }
    }
}
