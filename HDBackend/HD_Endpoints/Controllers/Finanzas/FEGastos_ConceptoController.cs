using HD.Security;
using HD_Cobranza.Capturas;
using HD_Finanzas.AccesoDatos;
using HD_Finanzas.Modelos.Gastos_Proyeccion;
using HD_Reporteria.Cobranza;
using HD_Reporteria;
using Microsoft.AspNetCore.Mvc;
using HD_Reporteria.Finanzas;
using HD_Cobranza.Reportes;
using HD_Reporteria.Finanzas.Excel;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FEGastos_ConceptoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FEGastos_ConceptoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GastosvsProyeccionbyTiempo(Fmdl_Gastos_Filtros vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_GastosvsProyeccion datos = new FAD_GastosvsProyeccion(CadenaConexion);
            var result = await datos.GetGastosvsProyeccion(vm, "1");
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReportePDF(Fmdl_Gastos_PDF vm)
        {

            try
            {
                RPT_Result documento = RPT_Finanzas_Gastos.Generar(vm);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(Fmdl_Gastos_PDF vm)
        {
            var docresult = await XLS_Gastos.Gastos(vm);
            return Ok(docresult);

        }
    }
}
