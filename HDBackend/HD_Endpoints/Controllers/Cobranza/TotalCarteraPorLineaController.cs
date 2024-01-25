using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Reportes;
using HD_Reporteria.Cobranza;
using HD_Reporteria;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class TotalCarteraPorLineaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public TotalCarteraPorLineaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idsucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_TotalCarteraPorLinea datos = new ADCob_TotalCarteraPorLinea(CadenaConexion);
            var result = await datos.Listado(idsucursal);
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel(int idsucursal)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_TotalCarteraPorLinea datos = new ADCob_TotalCarteraPorLinea(CadenaConexion);
            var result = await datos.Listado(idsucursal);
            var docResult = await XLSCob_TotalCartera_Linea.CrearExcel(result);
            return Ok(docResult);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteLineaPDF()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            //ADPedido_Impresion_View datos = new ADPedido_Impresion_View(CadenaConexion);
            //var result = await datos.Get(folio);


            try
            {
                RPT_Result documento = RPT_TotalCartera_PorLinea.Generar();

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
