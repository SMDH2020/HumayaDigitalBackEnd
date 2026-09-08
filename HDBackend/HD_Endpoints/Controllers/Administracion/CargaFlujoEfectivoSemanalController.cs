using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Administracion.Consultas;

namespace HD.Endpoints.Controllers.Administracion
{
    public class CargaFlujoEfectivoSemanalController : ControllerBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public CargaFlujoEfectivoSemanalController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> FlujoEfectivo(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Flujo_Efectivo_Semanal datos = new AD_Carga_Flujo_Efectivo_Semanal(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            usuario = 8919;
            var result = await datos.FlujoEfectivo(ejercicio, periodo);
            return Ok(result);
        }
    }
}
