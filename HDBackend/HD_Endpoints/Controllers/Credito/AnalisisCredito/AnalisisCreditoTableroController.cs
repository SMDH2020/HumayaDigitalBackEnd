using HD.Clientes.Consultas.SolicitudCredito_Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito.AnalisisCredito
{
    public class AnalisisCreditoTableroController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public AnalisisCreditoTableroController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetTablero()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_SCAnalisis_Tablero datos = new AD_SCAnalisis_Tablero(CadenaConexion);
            var result = await datos.Get(Sesion.usuario());
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetTableroMes(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_SCAnalisis_Tablero_Mes datos = new AD_SCAnalisis_Tablero_Mes(CadenaConexion);
            var result = await datos.Get(Sesion.usuario(), ejercicio, periodo);
            return Ok(result);

        }
    }
}
