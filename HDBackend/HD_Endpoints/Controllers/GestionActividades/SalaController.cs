using HD_GestionActividades.Consultas.Sala;
using HD_GestionActividades.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionActividades
{
    public class SalaController : MyBase   /*HEREDAR AL FINAL LA SEGURIDAD QUE SERIA: "public class SalaController :MyBase"*/
    {
        private readonly IConfiguration _configuracion;

        public SalaController(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }

        [HttpGet("listado")]
        public async Task<IActionResult> Listado()
        {
            // 🔹 Aquí se obtiene la cadena desde appsettings.json
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];

            AD_Sala ad = new AD_Sala(cadenaConexion);

            var resultado = await ad.Listado();

            return Ok(resultado);
        }

        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] mdl_Sala sala)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_Sala ad = new AD_Sala(cadenaConexion);

            var resultado = await ad.Guardar(sala);

            return Ok(resultado);
        }

        [HttpGet("Obtener/{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_Sala ad = new AD_Sala(cadenaConexion);

            var resultado = await ad.Obtener(id);

            return Ok(resultado);
        }
    }
}