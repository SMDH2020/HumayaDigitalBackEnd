using HD.Security;
using HD_Cobranza.Capturas.ConvenioPago;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza.ConvenioPago
{
    public class CPPredictivoClientesController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CPPredictivoClientesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCPClientesPredictivo datos = new ADCPClientesPredictivo(CadenaConexion);
            var result = await datos.Listado(idcliente.ToString());
            return Ok(result);

        }
    }
}
