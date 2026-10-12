using HD.Endpoints.Controllers;
using HD.Security;
using HD_GestionActividades.Consultas.SalaActividad;
using Microsoft.AspNetCore.Mvc;

namespace HD_GestionActividades.Controllers
{
    public class SalaActividadController : MyBase
    {
        private readonly IConfiguration _configuracion;
        private readonly ISesion _session;

        public SalaActividadController(IConfiguration configuracion, ISesion session)
        {
            _configuracion = configuracion;
            _session = session;
        }

        [HttpGet("Listado/{idSala}")]
        public async Task<IActionResult> Listado(short idSala)
        {
            try
            {
                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];

                AD_SalaActividad ad = new AD_SalaActividad(cadenaConexion);

                var resultado = await ad.Listado(idSala);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }


        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar(
            [FromQuery] short idSala,
            [FromQuery] short idActividad,
            [FromQuery] short idUsuario)
        {
            try
            {
                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];

                AD_SalaActividad ad = new AD_SalaActividad(cadenaConexion);

                await ad.Guardar(idSala, idActividad, idUsuario);

                return Ok(new { mensaje = "Actividad guardada correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("EliminarPorID")]
        public async Task<IActionResult> EliminarPorID([FromBody] EliminarActividadRequest model)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];

            AD_SalaActividad ad = new AD_SalaActividad(cadenaConexion);

            var resultado = await ad.EliminarPorID(
                model.IdRelSalaActividad,
                int.Parse(_session.usuario()),
                model.idSala
            );

            return Ok(resultado);
        }
    }
    // DTO
    public class EliminarActividadRequest
    {
        public int IdRelSalaActividad { get; set; }
        public int idSala { get; set; }
    }
}