using HD_Cobranza.Capturas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Ventas.Consultas;
using HD_Cobranza.Capturas.ConvenioPago;
namespace HD.Endpoints.Controllers.Cobranza
{
    public class ResponsableCobranzaporClienteController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ResponsableCobranzaporClienteController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> ObtenerResponsable(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Responsable_Cobranza_porCliente datos = new AD_Responsable_Cobranza_porCliente(CadenaConexion);
            var result = await datos.Cliente(idcliente);
            return Ok(result);
        }
    }
}
