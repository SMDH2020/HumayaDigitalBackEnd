using HD.Security;
using HD_Cobranza.Capturas.ConvenioPago;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza.ConvenioPago
{
    public class InfoclientesController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public InfoclientesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> informacion(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCPInfoClientes datos = new ADCPInfoClientes(CadenaConexion);
            var result = await datos.Obtener(idcliente);
            return Ok(result);

        }
    }
}
