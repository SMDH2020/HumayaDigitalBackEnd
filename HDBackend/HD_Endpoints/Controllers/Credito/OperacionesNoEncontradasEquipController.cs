using HD.Security;
using HD.Clientes.Consultas.Credito;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class OperacionesNoEncontradasEquipController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public OperacionesNoEncontradasEquipController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoOperaciones()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Operaciones_noEncontradas_Equip datos = new AD_Operaciones_noEncontradas_Equip(CadenaConexion);
            var result = await datos.operaciones();
            return Ok(result);
        }
    }
}
