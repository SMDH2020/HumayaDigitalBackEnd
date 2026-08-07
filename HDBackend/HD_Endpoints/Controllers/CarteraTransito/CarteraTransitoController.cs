using CarteraTransito.Consultas;
using CarteraTransito.Modelos;
using HD.Security;
using HD_Auditoria.Consultas.Justificaciones;
using HD_Cobranza.Capturas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.CarteraTransito
{
    public class CarteraTransitoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CarteraTransitoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> listado(int ejercicio, int periodo, string sucursal, string adr)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cartera_Transito_Listado datos = new AD_Cartera_Transito_Listado(CadenaConexion);
            var result = await datos.Listado(ejercicio, periodo, sucursal, adr);
            return Ok(result);
        }
    }
}
