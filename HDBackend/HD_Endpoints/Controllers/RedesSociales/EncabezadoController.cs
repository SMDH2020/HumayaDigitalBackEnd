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
    public class EncabezadoController : MyBase
    {
        private readonly IConfiguration _configuracion;
        private readonly ISesion _session;

        public EncabezadoController(IConfiguration configuracion, ISesion session)
        {
            _configuracion = configuracion;
            _session = session;
        }

        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar([FromForm] mdl_Encabezado encabezado)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_Encabezado ad = new AD_Encabezado(cadenaConexion);

            if (encabezado.Archivo != null)
            {
                using var ms = new MemoryStream();
                await encabezado.Archivo.CopyToAsync(ms);

                byte[] bytes = ms.ToArray();
                encabezado.ImagenBase64 = Convert.ToBase64String(bytes);
            }

            string folio = await ad.GuardarAsync(encabezado);

            return Ok(new
            {
                mensaje = "Guardado correctamente",
                folio
            });
        }

        [HttpPost("Editar")]
        public async Task<IActionResult> Editar([FromBody] mdl_Encabezado encabezado)
        {
            try
            {
                if (encabezado == null || string.IsNullOrEmpty(encabezado.Folio))
                    return BadRequest(new { mensaje = "Datos inválidos" });

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Encabezado ad = new AD_Encabezado(cadenaConexion);

                await ad.EditarAsync(encabezado);

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
                AD_Encabezado ad = new AD_Encabezado(cadenaConexion);

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
                AD_Encabezado ad = new AD_Encabezado(cadenaConexion);

                var resultado = await ad.ListadoAsync(folio);

                return Ok(resultado);
            }   
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("Calendario")]
        public async Task<IActionResult> Calendario()
        {
            try
            {
                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Encabezado ad = new AD_Encabezado(cadenaConexion);

                var resultado = await ad.CalendarioAsync();

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}   