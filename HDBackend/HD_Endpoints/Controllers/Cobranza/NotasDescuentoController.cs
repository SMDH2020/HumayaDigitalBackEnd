using HD.Security;
using HD_Cobranza.Capturas.NotasDescuento;
using HD_Cobranza.Consultas;
using HD_Cobranza.Modelos;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.ReporteMensajeria;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class NotasDescuentoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public NotasDescuentoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerNotas(string adr, string sucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Notas_Descuento datos = new AD_Notas_Descuento(CadenaConexion);
            var result = await datos.ObtenerNotas(adr, sucursal);
            return Ok(result);
        }
    }
}
