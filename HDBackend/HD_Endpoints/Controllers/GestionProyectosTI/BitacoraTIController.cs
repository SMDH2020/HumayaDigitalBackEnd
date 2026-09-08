using HD.Security;
using HD_GestionProyectosTI.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionProyectosTI
{
    [ApiController]
    [Route("api/[controller]")]
    public class BitacoraTIController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public BitacoraTIController(IConfiguration configuracion, ISesion sesion)
        {
            Configuracion = configuracion;
            Sesion = sesion;
        }

        private string CadenaConexion => Configuracion["ConnectionStrings:GestionProyectosTI"]!;

        // entidad: 'Solicitud' | 'Actividad'. El Admin ve el detalle técnico
        // completo; el frontend del Usuario debe mostrar solo una versión
        // resumida (cambios de estado, fecha y prioridad) filtrando en el
        // cliente por tipo_evento, o bien se agrega más adelante un endpoint
        // "resumido" dedicado si se requiere ocultar más detalle.
        [HttpGet("Historial")]
        public async Task<ActionResult> Historial(string entidad, int identidad)
        {
            int idusuario = int.Parse(Sesion.usuario());
            string rol = await new AD_RolTI(CadenaConexion).Obtener(idusuario);
            if (rol != "Admin")
                return Forbid();

            var result = await new AD_Bitacora(CadenaConexion).Historial(entidad, identidad);
            return Ok(result);
        }
    }
}
