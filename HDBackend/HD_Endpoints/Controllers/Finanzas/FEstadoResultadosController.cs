using HD.Security;
using HD_Finanzas.AccesoDatos;
using HD_Finanzas.AccesoDatos.Actions;
using HD_Finanzas.Modelos.Estado_Resultados;
using HD_Reporteria;
using HD_Reporteria.Finanzas;
using HD_Reporteria.Finanzas.Excel;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FEstadoResultadosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FEstadoResultadosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetEstadoResultadosByDireccionRolado(Fmdl_EstadoResultadosRolado prm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            string usuario = Sesion.usuario();
            FAD_EstadoResultados estadoresultados = new FAD_EstadoResultados(CadenaConexion);
            var result = await estadoresultados.GetEstadoResultadosByDireccionRolado(prm, usuario);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetEstadoResultadoGrafica(Fmdl_Estado_Resultados_Grafica_Filtro prm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_EstadiResultados_Grafica estadoresultados = new FAD_EstadiResultados_Grafica(CadenaConexion);
            var result = await estadoresultados.EstadoResultadosGrafica(prm);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReportePDF(Fmdl_EstadoResultados_PDF prm)
        {
            try
            {
                RPT_Result documento = RPT_Finanzas_EstadoResultados.Generar(prm);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(Fmdl_EstadoResultados_PDF vm)
        {
            var docresult = await XLS_EstadoResultados.EstadoResultados(vm);
            return Ok(docresult);

        }
    }
}
