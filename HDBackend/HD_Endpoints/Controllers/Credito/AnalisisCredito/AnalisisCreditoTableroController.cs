using HD.Clientes.Consultas.SolicitudCredito_Analisis;
using HD.Notifications.NotificacionesApp;
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

            string origen = Sesion.origen();
            if (Sesion.generarLog() == true && origen == "APP")
            {
                NE_Logs_App_HD log = new NE_Logs_App_HD(CadenaConexion);
                await log.Guardar("Navego al menu de solicitudes de credito", origen, Sesion.usuario());
            }

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

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetTableroCompleto()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_SCAnalisis_Tablero_Completo datos = new AD_SCAnalisis_Tablero_Completo(CadenaConexion);
            var result = await datos.Get(Sesion.usuario());
            return Ok(result);

        }
    }
}
