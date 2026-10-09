using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Capturas.ReporteRecuperacionCompleta;
using HD_Cobranza.Reportes;
using HD_Reporteria;
using HD_Reporteria.Cobranza;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.Dashboard;
using Postventa.Consultas.Indicadores;
using Postventa.Reportes;

namespace HD.Endpoints.Controllers.Postventa
{
    public class IndicadoresPosventasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public IndicadoresPosventasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Indicadores(int ejercicio_inicio, int ejercicio_fin, int periodo_inicio, int periodo_fin, string seccion, string sucursales, string adr)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Indicadores_Posventa datos = new AD_Indicadores_Posventa(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.ObtenerIndicadores(ejercicio_inicio, ejercicio_fin, periodo_inicio, periodo_fin, seccion, sucursales, adr);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirPDF(int ejercicio_inicio, int ejercicio_fin, int periodo_inicio, int periodo_fin, string seccion, string sucursales, string adr, string periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Indicadores_Posventa datos = new AD_Indicadores_Posventa(CadenaConexion);
            var result = await datos.ObtenerIndicadores(ejercicio_inicio, ejercicio_fin, periodo_inicio, periodo_fin, seccion, sucursales, adr);

            try
            {
                RPT_Result documento = RPTDesempeñoPorResponsable.GenerarPDF(result.Detalle, periodo);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(int ejercicio_inicio, int ejercicio_fin, int periodo_inicio, int periodo_fin, string seccion, string sucursales, string adr, string periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Indicadores_Posventa datos = new AD_Indicadores_Posventa(CadenaConexion);
            string usuario = Sesion.usuario();
            var result = await datos.ObtenerIndicadores(ejercicio_inicio, ejercicio_fin, periodo_inicio, periodo_fin, seccion, sucursales, adr);
            var docresult = await XLS_DesempeñoPorResponsable.GenerarExcel(result.Detalle, periodo);
            return Ok(docresult);

        }
    }
}
