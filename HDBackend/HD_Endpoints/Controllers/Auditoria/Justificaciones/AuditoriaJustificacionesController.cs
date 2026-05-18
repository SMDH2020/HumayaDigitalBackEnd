using HD.Security;
using HD_Auditoria.Consultas.Justificaciones;
using HD_Auditoria.Consultas.Programar_Inventario;
using HD_Auditoria.Modelos.Programar_Inventario;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Auditoria.Justificaciones
{
    public class AuditoriaJustificacionesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public AuditoriaJustificacionesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<IActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Justificaciones_Listado datos = new AD_Justificaciones_Listado(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.Listado(usuario);
            return Ok(result);

        }
    }
}
