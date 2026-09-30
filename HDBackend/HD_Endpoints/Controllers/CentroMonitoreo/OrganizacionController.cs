using HD_CentroMonitoreo.Consultas.Organizacion;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.CentroMonitoreo
{
    public class OrganizacionController : MyBase
    {
        private readonly IConfiguration _configuracion;

        public OrganizacionController(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }

        [HttpGet("listado")]
        public async Task<IActionResult> Listado()
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_Organizacion ad = new AD_Organizacion(cadenaConexion);

            var resultado = await ad.Listado();

            return Ok(resultado);
        }

        [HttpGet("Detalle/{jd_org_id}")]
        public async Task<IActionResult> Detalle(string jd_org_id)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_Organizacion ad = new AD_Organizacion(cadenaConexion);

            var resultado = await ad.Detalle(jd_org_id);

            return Ok(resultado);
        }
    }
}