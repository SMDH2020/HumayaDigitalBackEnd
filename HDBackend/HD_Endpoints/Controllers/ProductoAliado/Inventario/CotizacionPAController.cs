using HD.Security;
using HD_Reporteria;
using HD_Reporteria.ProductoAliado;
using Microsoft.AspNetCore.Mvc;
using ProductoAliado.Consultas.Cotizacion;

namespace HD.Endpoints.Controllers.ProductoAliado.Inventario
{
    public class CotizacionPAController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CotizacionPAController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ImprimirCotizacionPDF(int idinventario)
        {
            // Concatenar todos los idinventario en una cadena separada por comas
            //string idinventario = string.Join(",", mdl.datosActualizados.Select(r => r.idinventario.ToString()));

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cotizacion_PA_PDF datos = new AD_Cotizacion_PA_PDF(CadenaConexion);
            var result = await datos.Cotizacion(idinventario);

            try
            {
                RPT_Result documento = RPT_Listado_Precios_Corto.GenerarPDF(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }
        }
    }
}
