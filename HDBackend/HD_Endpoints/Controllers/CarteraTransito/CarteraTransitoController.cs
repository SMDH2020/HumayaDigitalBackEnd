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
        public async Task<ActionResult> listado(int ejercicio, int periodo, string sucursal, string adr,string pendientes)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cartera_Transito_Listado datos = new AD_Cartera_Transito_Listado(CadenaConexion);
            var result = await datos.Listado(ejercicio, periodo, sucursal, adr,pendientes);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> actualizar(int ejercicio, int periodo, int ejerciciotransito, int periodotransito)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cartera_Transito_Actualizar datos = new AD_Cartera_Transito_Actualizar(CadenaConexion);
            var result = await datos.actualizar(ejercicio, periodo, ejerciciotransito, periodotransito);
            return Ok(result);
        }
    }
}
