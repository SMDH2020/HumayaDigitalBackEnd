using HD_GestionActividades.Consultas.GrupoActividades;
using HD_GestionActividades.Consultas.Sala;
using HD_GestionActividades.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionActividades
{
    public class GrupoActividadesController : MyBase
    {
        private readonly IConfiguration _configuracion;

        public GrupoActividadesController(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }

        [HttpGet("listado")]
        public async Task<IActionResult> Listado()
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];

            AD_GrupoActividades ad = new AD_GrupoActividades(cadenaConexion);

            var resultado = await ad.Listado();

            return Ok(resultado);
        }

        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] mdl_GrupoActividades grupo)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];

            AD_GrupoActividades ad = new AD_GrupoActividades(cadenaConexion);

            var resultado = await ad.Guardar(grupo);

            return Ok(resultado);
        }

        [HttpGet("Obtener/{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_GrupoActividades ad = new AD_GrupoActividades(cadenaConexion);

            var resultado = await ad.Obtener(id); // Este método hay que crear en el AD
            if (resultado == null)
                return NotFound();

            return Ok(resultado);
        }
    }
}