using HD.Security;
using HD_Mensajeria.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Mensajeria
{
    public class MensajeriaEnviadaController : MyBase
    {

        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public MensajeriaEnviadaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> obtenerMensajes(string fechainicio, string fechafin, string seccion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_MEnsajeria_Enviada_Listado datos = new AD_MEnsajeria_Enviada_Listado(CadenaConexion);
            var result = await datos.mensajeriaListado(fechainicio, fechafin, seccion);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> obtenerDetalle(string fecha, string seccion)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_MEnsajeria_Enviada_Listado datos = new AD_MEnsajeria_Enviada_Listado(CadenaConexion);
            var result = await datos.mensajeriaDetalle(fecha, seccion);
            return Ok(result);
        }
    }
}
