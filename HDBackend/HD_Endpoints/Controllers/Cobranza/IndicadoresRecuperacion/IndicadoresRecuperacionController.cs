using HD.Security;
using HD_Cobranza.Capturas.IndicadoresRecuperacion;
using HD_Cobranza.Modelos.IndicadoresRecuperacion;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza.IndicadoresRecuperacion
{
    public class IndicadoresRecuperacionController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public IndicadoresRecuperacionController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Guardar(List<mdl_Indicadores_Recuperacion> mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Indicador_Recuperacion_Guardar datos = new AD_Indicador_Recuperacion_Guardar(CadenaConexion);
            string usuarioSesion = Sesion.usuario();
            foreach (var indicador in mdl)
            {
                indicador.usuario = usuarioSesion;

                // Guardar cada indicador en la base de datos
                await datos.Guardar(indicador);
            }
            return Ok(new { mensaje = "Datos Cargados cone exito" });
        }


        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> Listado(int ejercicio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Indicador_Recuperacion_Listado datos = new AD_Indicador_Recuperacion_Listado(CadenaConexion);
            var result = await datos.Listado(ejercicio);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarSemaforo(List<mdl_Indicadores_Semaforo_Recuperacion> mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Indicador_Semaforo_Recuperacion_Guardar datos = new AD_Indicador_Semaforo_Recuperacion_Guardar(CadenaConexion);
            string usuarioSesion = Sesion.usuario();
            foreach (var indicador in mdl)
            {
                indicador.usuario = usuarioSesion;

                // Guardar cada indicador en la base de datos
                await datos.Guardar(indicador);
            }
            return Ok(new { mensaje = "Datos Cargados cone exito" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> ListadoSemaforo(int ejercicio, string? tipo_cartera)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Indicadores_Semaforo_Recuperacion_Listado datos = new AD_Indicadores_Semaforo_Recuperacion_Listado(CadenaConexion);
            var result = await datos.Listado(ejercicio, tipo_cartera);
            return Ok(result);
        }
    }
}
