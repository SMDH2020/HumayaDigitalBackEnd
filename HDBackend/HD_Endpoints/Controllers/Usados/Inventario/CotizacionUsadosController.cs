using HD.Notifications.NotificacionesApp;
using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Reporteria;
using HD_Reporteria.Cobranza;
using HD_Reporteria.Usados;
using Microsoft.AspNetCore.Mvc;
using Usados.Consultas.Inventario;
using Usados.Consultas.Usados;
using Usados.Modelos.ListadoPrecios;
using Usados.Modelos.Usados;

namespace HD.Endpoints.Controllers.Usados.Inventario
{
    public class CotizacionUsadosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public CotizacionUsadosController(IConfiguration configuration, ISesion sesion)
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
            AD_Cotizacion_Usados_PDF datos = new AD_Cotizacion_Usados_PDF(CadenaConexion);
            var result = await datos.Cotizacion(idinventario);

            string origen = Sesion.origen();
            if (Sesion.generarLog() == true && origen == "APP")
            {
                NE_Logs_App_HD log = new NE_Logs_App_HD(CadenaConexion);
                await log.Guardar($"Compartio una cotizacion del idinventario: {idinventario}", origen, Sesion.usuario());
            }

            try
            {
                RPT_Result documento = RPT_Cotizacion_Usados.GenerarPDF(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }
        }
    }
}
