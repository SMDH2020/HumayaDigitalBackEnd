using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionCobranza
{
    public class MostarContactoClienteController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public MostarContactoClienteController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ContactoCliente(int idcliente, string medio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Mostrar_Contacto_Cliente datos = new AD_Mostrar_Contacto_Cliente(CadenaConexion);
            var result = await datos.Contacto(idcliente, medio);
            return Ok(result);
        }
    }
}
