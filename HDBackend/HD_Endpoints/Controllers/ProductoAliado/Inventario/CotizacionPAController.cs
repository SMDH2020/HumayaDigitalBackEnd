using HD.Notifications.NotificacionesApp;
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

            string origen = Sesion.origen();
            if (Sesion.generarLog() == true && origen == "APP")
            {
                NE_Logs_App_HD log = new NE_Logs_App_HD(CadenaConexion);
                await log.Guardar($"Se compartio una cotizacion de Poducto Aliado del idinventario: {idinventario}", origen, Sesion.usuario());
            }

            try
            {
                RPT_Result documento = RPT_Cotizacion_ProductoAliado.GenerarPDF(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }
        }
    }
}
