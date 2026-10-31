using HD.Security;
using HD_Finanzas.AccesoDatos;
using HD_Finanzas.Modelos.Estado_Resultados;
using HD_Finanzas.Modelos.Linea_Negocio;
using HD_Reporteria.Finanzas;
using HD_Reporteria;
using Microsoft.AspNetCore.Mvc;
using HD_Finanzas.Modelos.Gastos_Proyeccion;
using HD_Reporteria.Finanzas.Excel;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FELineaNegocioController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FELineaNegocioController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetEsquemaByLineadeNegocio(Fmdl_Linea_negocio_filtros vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_LineaNegocio datos = new FAD_LineaNegocio(CadenaConexion);
            var result = await datos.GetEsquemaByLineadeNegocio(vm);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReportePDF(Fmdl_Linea_Negocio_Ventas_PDF vm)
        {

            try
            {
                RPT_Result documento = RPT_Finanzas_Linea_Negocio_Ventas.Generar(vm);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }


        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(Fmdl_Linea_Negocio_Ventas_PDF vm)
        {
            var docresult = await XLS_Linea_Negocio_Ventas.lineaNegocio(vm);
            return Ok(docresult);

        }
    }

}
