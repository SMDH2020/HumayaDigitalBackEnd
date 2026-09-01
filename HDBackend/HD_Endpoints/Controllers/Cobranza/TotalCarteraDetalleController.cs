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
        public async Task<ActionResult> GenerarExcel(int idsucursal, string linea)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_TotalCarteraDetalle datos = new ADCob_TotalCarteraDetalle(CadenaConexion);
            var result = await datos.Listado(idsucursal, linea, Sesion.usuario());
            var docResult = await XLSCob_TotalCartera_Detalle.CrearExcel(result, linea);

            return Ok(docResult);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GenerarExcelCliente(int cliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_TotalCarteraDetalle datos = new ADCob_TotalCarteraDetalle(CadenaConexion);
            var result = await datos.ListadoPorCliente(cliente);
            var docResult = await XLSCob_TotalCartera_Detalle_Cliente.CrearExcel(result);
            return Ok(docResult);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteDetallePDF(int idsucursal, string linea)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_TotalCarteraDetalle datos = new ADCob_TotalCarteraDetalle(CadenaConexion);
            var result = await datos.Listado(idsucursal, linea, Sesion.usuario());


            try
            {
                RPT_Result documento = RPT_TotalCartera_Detalle.Generar(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReporteDetalleClientePDF(int cliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADCob_TotalCarteraDetalle datos = new ADCob_TotalCarteraDetalle(CadenaConexion);
            var result = await datos.ListadoPorCliente(cliente);


            try
            {
                RPT_Result documento = RPT_TotalCartera_Detalle_Cliente.Generar(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
