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

        public ActividadController(IConfiguration configuracion)
        {
            _configuracion = configuracion;
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
    }
} 