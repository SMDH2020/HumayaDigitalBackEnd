using Dapper;
using HD.AccesoDatos;
using HD.Endpoints.Controllers;
using HD.Security;
using HD_RedesSociales.Consultas;
using HD_RedesSociales.Modelos;
using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Data.SqlClient;

namespace HD.Endpoints.Controllers.RedesSociales
{
    [ApiController]
    [Route("api/[controller]")]
    public class Reels_EscenasController : MyBase
    {
        private readonly IConfiguration _configuracion;
        private readonly ISesion _session;

        public Reels_EscenasController(IConfiguration configuracion, ISesion session)
        {
            _configuracion = configuracion;
            _session = session;
        }

        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] mdl_Reels_Escena escena)
        {
            try
            {
                if (escena == null || string.IsNullOrEmpty(escena.Folio))
                    return BadRequest(new { mensaje = "Datos inválidos" });

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Reels_Escenas ad = new AD_Reels_Escenas(cadenaConexion);

                await ad.GuardarAsync(escena);

                return Ok(new { mensaje = "Guardado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("EliminarTodas")]
        public async Task<IActionResult> EliminarTodas([FromBody] string folio)
        {
            try
            {
                if (string.IsNullOrEmpty(folio))
                    return BadRequest(new { mensaje = "Folio inválido" });

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Reels_Escenas ad = new AD_Reels_Escenas(cadenaConexion);

                await ad.EliminarTodasAsync(folio);

                return Ok(new { mensaje = "Eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("Listado")]
        public async Task<IActionResult> Listado([FromQuery] string folio)
        {
            try
            {
                if (string.IsNullOrEmpty(folio))
                    return BadRequest(new { mensaje = "Folio inválido" });

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Reels_Escenas ad = new AD_Reels_Escenas(cadenaConexion);

                var resultado = await ad.ListadoAsync(folio);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}