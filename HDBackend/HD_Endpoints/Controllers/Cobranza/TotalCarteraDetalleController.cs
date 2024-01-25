using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Reportes;
using HD_Reporteria.Cobranza;
using HD_Reporteria;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class TotalCarteraDetalleController :MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public TotalCarteraDetalleController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idsucursal,string linea)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_TotalCarteraDetalle datos = new ADCob_TotalCarteraDetalle(CadenaConexion);
            var result = await datos.Listado(idsucursal,linea,Sesion.usuario());
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoPorCliente(int cliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_TotalCarteraDetalle datos = new ADCob_TotalCarteraDetalle(CadenaConexion);
            var result = await datos.ListadoPorCliente(cliente);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcel()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            //ADCob_TotalCarteraPorLinea datos = new ADCob_TotalCarteraPorLinea(CadenaConexion);
            //var result = await datos.Listado(idsucursal);
            var docResult = await XLSCob_TotalCartera_Detalle.CrearExcel();
            return Ok(docResult);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcelCliente()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            //ADCob_TotalCarteraPorLinea datos = new ADCob_TotalCarteraPorLinea(CadenaConexion);
            //var result = await datos.Listado(idsucursal);
            var docResult = await XLSCob_TotalCartera_Detalle_Cliente.CrearExcel();
            return Ok(docResult);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteDetallePDF()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            //ADPedido_Impresion_View datos = new ADPedido_Impresion_View(CadenaConexion);
            //var result = await datos.Get(folio);


            try
            {
                RPT_Result documento = RPT_TotalCartera_Detalle.Generar();

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteDetalleClientePDF()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            //ADPedido_Impresion_View datos = new ADPedido_Impresion_View(CadenaConexion);
            //var result = await datos.Get(folio);


            try
            {
                RPT_Result documento = RPT_TotalCartera_Detalle_Cliente.Generar();

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
