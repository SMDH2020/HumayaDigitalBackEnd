using HD.Security;
using HD_Ventas.Consultas.AnalisisInventarioModelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class AnalisisInventarioModelosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public AnalisisInventarioModelosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Reporte(int ejercicio_inicio, int periodo_inicio, int ejercicio_fin, int periodo_fin, int comparativo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_AnalisisInventarioModelos_Reporte datos = new AD_AnalisisInventarioModelos_Reporte(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Reporte(ejercicio_inicio, periodo_inicio, ejercicio_fin, periodo_fin, comparativo, usuario);
            return Ok(result);
        }
    }
}
