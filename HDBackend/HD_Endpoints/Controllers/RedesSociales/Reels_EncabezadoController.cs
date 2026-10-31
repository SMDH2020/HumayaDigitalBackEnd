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
    public class Reels_EncabezadoController : MyBase
    {
        private readonly IConfiguration _configuracion;
        private readonly ISesion _session;

        public Reels_EncabezadoController(IConfiguration configuracion, ISesion session)
        {
            _configuracion = configuracion;
            _session = session;
        }

        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] mdl_Reels_Encabezado reel)
        {
            try
            {
                if (reel == null || string.IsNullOrEmpty(reel.Modo))
                    return BadRequest(new { mensaje = "Datos inválidos" });

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Reels_Encabezado ad = new AD_Reels_Encabezado(cadenaConexion);

                string folio = await ad.GuardarAsync(reel);

                return Ok(new { mensaje = "Guardado correctamente", folio });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("Editar")]
        public async Task<IActionResult> Editar([FromBody] mdl_Reels_Encabezado reel)
        {
            try
            {
                if (reel == null || string.IsNullOrEmpty(reel.Folio))
                    return BadRequest(new { mensaje = "Datos inválidos" });

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Reels_Encabezado ad = new AD_Reels_Encabezado(cadenaConexion);

                await ad.EditarAsync(reel);

                return Ok(new { mensaje = "Editado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("Eliminar")]
        public async Task<IActionResult> Eliminar([FromBody] string folio)
        {
            try
            {
                if (string.IsNullOrEmpty(folio))
                    return BadRequest(new { mensaje = "Folio inválido" });

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Reels_Encabezado ad = new AD_Reels_Encabezado(cadenaConexion);

                await ad.EliminarAsync(folio);

                return Ok(new { mensaje = "Eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("Listado")]
        public async Task<IActionResult> Listado([FromQuery] string? folio = null)
        {
            try
            {
                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Reels_Encabezado ad = new AD_Reels_Encabezado(cadenaConexion);

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