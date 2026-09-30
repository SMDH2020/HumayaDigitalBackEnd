using HD.Security;
using HD_GestionActividades.Consultas.Actividad;
using HD_GestionActividades.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionActividades
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActividadController : MyBase
    {
        private readonly IConfiguration _configuracion;
        private readonly ISesion _session;

        public ActividadController(IConfiguration configuracion, ISesion session)
        {
            _configuracion = configuracion;
            _session = session;
        }

        [HttpGet("listado")]
        public async Task<IActionResult> Listado(
            int? idGrupoActividades,
            string? nombreActividad,
            bool? estado)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];

            AD_Actividad ad = new AD_Actividad(cadenaConexion);

            var resultado = await ad.Listado(
                idGrupoActividades,
                nombreActividad,
                estado
            );

            return Ok(resultado);
        }
        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] mdl_Actividad actividad)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];

            AD_Actividad ad = new AD_Actividad(cadenaConexion);

            var resultado = await ad.Guardar(actividad);

            return Ok(resultado);
        }

        [HttpGet("Obtener/{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_Actividad ad = new AD_Actividad(cadenaConexion);

            var resultado = await ad.Obtener(id); // Este método hay que crear en el AD
            if (resultado == null)
                return NotFound();

            return Ok(resultado);
        }

        [HttpGet("ActividadesPorSala/{idSala}")]
        public async Task<IActionResult> ActividadesPorSala(int idSala)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];

            AD_Actividad ad = new AD_Actividad(cadenaConexion);

            var resultado = await ad.ActividadesPorSala(idSala);

            return Ok(resultado);
        }

        // Configuración de recurrencia (tickets automáticos) de una
        // actividad ya existente -- endpoints nuevos, no tocan Guardar
        // ni Listado/Obtener de arriba.
        [HttpPost("GuardarRecurrencia")]
        public async Task<IActionResult> GuardarRecurrencia([FromBody] mdl_ActividadRecurrencia recurrencia)
        {
            try
            {
                if (recurrencia == null || recurrencia.idActividad == 0)
                    return BadRequest(new { mensaje = "Datos inválidos" });

                if (recurrencia.esRecurrente &&
                    (recurrencia.idSalaRecurrente == null ||
                     recurrencia.idUsuarioRecurrente == null ||
                     string.IsNullOrWhiteSpace(recurrencia.frecuenciaRecurrente) ||
                     recurrencia.diaRecurrente == null))
                {
                    return BadRequest(new { mensaje = "Para marcar la actividad como recurrente faltan datos: sala, usuario, frecuencia y día" });
                }

                recurrencia.usuario = int.Parse(_session.usuario());

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Actividad ad = new AD_Actividad(cadenaConexion);

                await ad.GuardarRecurrencia(recurrencia);

                return Ok(new { mensaje = "Recurrencia guardada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("ObtenerRecurrencia/{idActividad}")]
        public async Task<IActionResult> ObtenerRecurrencia(int idActividad)
        {
            try
            {
                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Actividad ad = new AD_Actividad(cadenaConexion);

                var resultado = await ad.ObtenerRecurrencia(idActividad);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("ListadoRecurrencia")]
        public async Task<IActionResult> ListadoRecurrencia()
        {
            try
            {
                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Actividad ad = new AD_Actividad(cadenaConexion);

                var resultado = await ad.ListadoRecurrencia();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
} 