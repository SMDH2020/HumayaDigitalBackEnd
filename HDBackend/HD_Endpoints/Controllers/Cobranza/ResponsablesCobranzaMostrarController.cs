using HD_Cobranza.Capturas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Ventas.Consultas;
using HD_Cobranza.Capturas.ConvenioPago;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class ResponsablesCobranzaMostrarController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ResponsablesCobranzaMostrarController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> Obtener_Responsables()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Responsables_Cobranza_Mostrar datos = new AD_Responsables_Cobranza_Mostrar(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);
        }
    }
}
