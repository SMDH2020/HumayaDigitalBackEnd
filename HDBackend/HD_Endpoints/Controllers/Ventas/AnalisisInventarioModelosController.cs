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
        // sucursal y adr: ids separados por coma ("1,5,8"); "0" = todas
        public async Task<ActionResult> Reporte(int ejercicio, int periodo_inicio_tendencia, int periodo_fin_tendencia, int periodo_inicio_resto, int periodo_fin_resto, int periodo_inicio_proyeccion, int periodo_fin_proyeccion, int comparativo, string sucursal = "0", string adr = "0")
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_AnalisisInventarioModelos_Reporte datos = new AD_AnalisisInventarioModelos_Reporte(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Reporte(ejercicio, periodo_inicio_tendencia, periodo_fin_tendencia, periodo_inicio_resto, periodo_fin_resto, periodo_inicio_proyeccion, periodo_fin_proyeccion, comparativo, sucursal, adr, usuario);
            return Ok(result);
        }
    }
}
