using Dapper;
using HD_GestionActividades.Consultas.SalaResponsable;
using HD_GestionActividades.Modelos;
using Microsoft.AspNetCore.Mvc;
using HD.AccesoDatos;
using HD.Security;

namespace HD.Endpoints.Controllers.GestionActividades
{

    public class SalaResponsableController : MyBase
    {
        private readonly IConfiguration _configuracion;
        private readonly ISesion _session;

        public SalaResponsableController(IConfiguration configuracion, ISesion session)
        {
            _configuracion = configuracion; 
            _session = session;
        }

        
        [HttpGet("Listado/{idSala}")]
        public async Task<IActionResult> Listado(short idSala)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];

            AD_SalaResponsable ad = new AD_SalaResponsable(cadenaConexion);

            var resultado = await ad.Listado(idSala);

            return Ok(resultado);
        }

        
        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] mdl_SalaResponsable model)
        {
            try
            {
                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];

                AD_SalaResponsable ad = new AD_SalaResponsable(cadenaConexion);

                var resultado = await ad.Guardar(model);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("EliminarPorID")]
        public async Task<IActionResult> EliminarPorID([FromBody] EliminarResponsableRequest model)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_SalaResponsable ad = new AD_SalaResponsable(cadenaConexion);

            var resultado = await ad.EliminarPorID(model.IdRelSalaResponsable, int.Parse(_session.usuario()), model.idSala);

            return Ok(resultado);
        }

        [HttpGet("EmpleadosDropDown")]
        public async Task<IActionResult> EmpleadosDropDown()
        {
            try
            {
                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_SalaResponsable ad = new AD_SalaResponsable(cadenaConexion);

                var resultado = await ad.EmpleadosDropDown();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
    // DTO
    public class EliminarResponsableRequest
    {
        public int IdRelSalaResponsable { get; set; }
        public int idSala { get; set; }
    }
}