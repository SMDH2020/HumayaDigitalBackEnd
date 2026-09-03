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
    public class AvataresController : MyBase
    {
        private readonly IConfiguration _configuracion;
        private readonly ISesion _session;

        public AvataresController(IConfiguration configuracion, ISesion session)
        {
            _configuracion = configuracion;
            _session = session;
        }

        [HttpGet("Listado")]
        public async Task<IActionResult> Listado([FromQuery] bool? activo = null)
        {
            try
            {
                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_Avatares ad = new AD_Avatares(cadenaConexion);

                var resultado = await ad.ListadoAsync(activo);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}