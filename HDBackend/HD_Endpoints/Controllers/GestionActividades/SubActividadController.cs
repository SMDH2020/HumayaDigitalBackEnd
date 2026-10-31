using HD.Security;
using HD_GestionActividades.Consultas.SubActividad;
using HD_GestionActividades.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionActividades
{
    // Catálogo (plantilla) de subactividades por actividad -- el checklist
    // que se clona a cada ticket que se crea para esa actividad. Endpoints
    // nuevos, independientes de ActividadController (no se toca).
    [Route("api/[controller]")]
    [ApiController]
    public class SubActividadController : MyBase
    {
        private readonly IConfiguration _configuracion;
        private readonly ISesion _session;

        public SubActividadController(IConfiguration configuracion, ISesion session)
        {
            _configuracion = configuracion;
            _session = session;
        }

        [HttpGet("Listado/{idActividad}")]
        public async Task<IActionResult> Listado(int idActividad)
        {
            try
            {
                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_SubActividad ad = new AD_SubActividad(cadenaConexion);

                var resultado = await ad.Listado(idActividad);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] mdl_SubActividad subActividad)
        {
            try
            {
                if (subActividad == null || subActividad.idActividad == 0 || string.IsNullOrWhiteSpace(subActividad.nombreSubActividad))
                    return BadRequest(new { mensaje = "Datos inválidos" });

                subActividad.usuario = int.Parse(_session.usuario());

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_SubActividad ad = new AD_SubActividad(cadenaConexion);

                var resultado = await ad.Guardar(subActividad);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("EliminarPorID")]
        public async Task<IActionResult> EliminarPorID([FromBody] int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest(new { mensaje = "ID inválido" });

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_SubActividad ad = new AD_SubActividad(cadenaConexion);

                await ad.EliminarPorID(id, int.Parse(_session.usuario()));

                return Ok(new { mensaje = "Subactividad eliminada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}
