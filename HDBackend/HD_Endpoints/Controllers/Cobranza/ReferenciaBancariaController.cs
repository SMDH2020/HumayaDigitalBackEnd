using HD.Notifications.NotificacionesApp;
using HD.Security;
using HD_Cobranza.Capturas.ConvenioPago;
using HD_Cobranza.Capturas.ReferenciasBancarias;
using HD_Cobranza.Modelos.ConvenioPago;
using HD_Reporteria;
using HD_Reporteria.Cobranza;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class ReferenciaBancariaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ReferenciaBancariaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Clientes_RB datos = new AD_Listado_Clientes_RB(CadenaConexion);
            var result = await datos.Listado();

            string origen = Sesion.origen();
            if (Sesion.generarLog() == true && origen == "APP")
            {
                NE_Logs_App_HD log = new NE_Logs_App_HD(CadenaConexion);
                await log.Guardar("Navego al menu de referencias bancarias", origen, Sesion.usuario());
            }

            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ReportePDF(string idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_RB_Obtener_Referencia datos = new AD_Clientes_RB_Obtener_Referencia(CadenaConexion);
            var result = await datos.obtenerReferencia(idcliente);

            string origen = Sesion.origen();
            if (Sesion.generarLog() == true && origen == "APP")
            {
                NE_Logs_App_HD log = new NE_Logs_App_HD(CadenaConexion);
                await log.Guardar($"Reviso la referencia bancaria del cliente: {idcliente}", origen, Sesion.usuario());
            }

            try
            {
                RPT_Result documento = RPT_ClientesRB_Referencia.GenerarReferenciaBancaria(result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }

        }
    }
}
