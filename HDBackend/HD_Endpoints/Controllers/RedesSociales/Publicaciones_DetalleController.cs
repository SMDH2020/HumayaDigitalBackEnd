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
    public class Publicaciones_DetalleController : MyBase
    {
        private readonly IConfiguration _configuracion;
        private readonly ISesion _session;

        public Publicaciones_DetalleController(IConfiguration configuracion, ISesion session)
        {
            _configuracion = configuracion;
            _session = session;
        }

        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] mdl_Publicaciones_Detalle detalle)
        {
            try
            {
                if (detalle == null || string.IsNullOrEmpty(detalle.Folio))
                    return BadRequest(new { mensaje = "Datos inválidos" });

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Publicaciones_Detalle ad = new AD_Publicaciones_Detalle(cadenaConexion);

                int consecutivo = await ad.GuardarAsync(detalle);

                return Ok(new { mensaje = "Guardado correctamente", consecutivo });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("Editar")]
        public async Task<IActionResult> Editar([FromBody] mdl_Publicaciones_Detalle detalle)
        {
            try
            {
                if (detalle == null || string.IsNullOrEmpty(detalle.Folio) || detalle.Consecutivo == 0)
                    return BadRequest(new { mensaje = "Datos inválidos" });

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Publicaciones_Detalle ad = new AD_Publicaciones_Detalle(cadenaConexion);

                await ad.EditarAsync(detalle);

                return Ok(new { mensaje = "Editado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("Eliminar")]
        public async Task<IActionResult> Eliminar([FromBody] mdl_Publicaciones_Detalle detalle)
        {
            try
            {
                if (detalle == null || string.IsNullOrEmpty(detalle.Folio))
                    return BadRequest(new { mensaje = "Datos inválidos" });

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Publicaciones_Detalle ad = new AD_Publicaciones_Detalle(cadenaConexion);

                await ad.EliminarAsync(detalle.Folio, detalle.Consecutivo == 0 ? null : detalle.Consecutivo);

                return Ok(new { mensaje = "Eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("Listado")]
        public async Task<IActionResult> Listado([FromQuery] string? folio = null, [FromQuery] int? consecutivo = null)
        {
            try
            {
                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Publicaciones_Detalle ad = new AD_Publicaciones_Detalle(cadenaConexion);

                var resultado = await ad.ListadoAsync(folio, consecutivo);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}