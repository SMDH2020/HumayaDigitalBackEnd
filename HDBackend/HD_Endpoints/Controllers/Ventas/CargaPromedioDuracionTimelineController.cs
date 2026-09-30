using HD_Ventas.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class CargaPromedioDuracionTimelineController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public CargaPromedioDuracionTimelineController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> PromedioTimeline(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Promedio_Duracion_Timeline datos = new AD_Carga_Promedio_Duracion_Timeline(CadenaConexion);
            var result = await datos.Promedio(ejercicio, periodo);
            return Ok(result);
        }
    }
}
