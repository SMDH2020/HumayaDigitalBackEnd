using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionCobranza
{
    public class ObtenerDatosContactoClienteController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ObtenerDatosContactoClienteController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DatosContacto(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Datos_Contacto_Cliente datos = new AD_Obtener_Datos_Contacto_Cliente(CadenaConexion);
            var result = await datos.Datos(idcliente);
            return Ok(result);
        }
    }
}
