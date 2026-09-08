using HD_CentroMonitoreo.Consultas.Maquina;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers
{
    public class MaquinaController : MyBase
    {
        private readonly IConfiguration _configuracion;

        public MaquinaController(IConfiguration configuracion)
        {
            _configuracion = configuracion;
        }

        [HttpGet("PorOrganizacion/{jd_org_id}")]
        public async Task<IActionResult> PorOrganizacion(string jd_org_id)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_Maquina ad = new AD_Maquina(cadenaConexion);

            var resultado = await ad.PorOrganizacion(jd_org_id);

            return Ok(resultado);
        }

        [HttpGet("Detalle/{jd_machine_id}")]
        public async Task<IActionResult> Detalle(string jd_machine_id)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_Maquina ad = new AD_Maquina(cadenaConexion);

            var resultado = await ad.Detalle(jd_machine_id);

            return Ok(resultado);
        }

        [HttpGet("EstadoPorMaquina/{maquina_id}")]
        public async Task<IActionResult> EstadoPorMaquina(string maquina_id)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_Maquina ad = new AD_Maquina(cadenaConexion);

            var resultado = await ad.EstadoPorMaquina(maquina_id);

            return Ok(resultado);
        }

        [HttpGet("AlertasPorMaquina/{maquina_id}")]
        public async Task<IActionResult> AlertasPorMaquina(string maquina_id)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_Maquina ad = new AD_Maquina(cadenaConexion);

            var resultado = await ad.AlertasPorMaquina(maquina_id);

            return Ok(resultado);
        }

        [HttpGet("RecorridoPorFecha/{jd_machine_id}")]
        public async Task<IActionResult> RecorridoPorFecha(string jd_machine_id, [FromQuery] DateTime fecha)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_Maquina ad = new AD_Maquina(cadenaConexion);

            var resultado = await ad.RecorridoPorFecha(jd_machine_id, fecha);

            return Ok(resultado);
        }










        [HttpGet("RecorridoPorRango/{jd_machine_id}")]
        public async Task<IActionResult> RecorridoPorRango(string jd_machine_id, [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_Maquina ad = new AD_Maquina(cadenaConexion);

            var resultado = await ad.RecorridoPorRango(jd_machine_id, desde, hasta);

            return Ok(resultado);
        }

        [HttpGet("HorasMotorPorRango/{jd_machine_id}")]
        public async Task<IActionResult> HorasMotorPorRango(string jd_machine_id, [FromQuery] DateTime? desde, [FromQuery] DateTime? hasta)
        {
            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_Maquina ad = new AD_Maquina(cadenaConexion);

            var resultado = await ad.HorasMotorPorRango(jd_machine_id, desde, hasta);

            return Ok(resultado);
        }

    }
}