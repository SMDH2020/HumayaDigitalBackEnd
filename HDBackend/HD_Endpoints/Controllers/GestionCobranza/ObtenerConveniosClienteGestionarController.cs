using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionCobranza
{
    public class ObtenerConveniosClienteGestionarController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ObtenerConveniosClienteGestionarController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Convenios(int idcliente, int card)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Convenios_Cliente_Gestionar datos = new AD_Obtener_Convenios_Cliente_Gestionar(CadenaConexion);
            var result = await datos.ObtenerConvenios(idcliente, card);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ConveniosOperacion(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Convenios_Cliente_Gestionar datos = new AD_Obtener_Convenios_Cliente_Gestionar(CadenaConexion);
            var result = await datos.ObtenerConveniosOperacion(idcliente);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ConveniosRevolvente(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Convenios_Cliente_Gestionar datos = new AD_Obtener_Convenios_Cliente_Gestionar(CadenaConexion);
            var result = await datos.ObtenerConveniosRevolvente(idcliente);
            return Ok(result);
        }

    }
}
