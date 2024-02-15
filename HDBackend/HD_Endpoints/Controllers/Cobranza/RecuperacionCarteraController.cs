using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Capturas.RecuperacionCartera;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.RecuperacionCartera;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class RecuperacionCarteraController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public RecuperacionCarteraController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener(RC_mdl_view obj)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADREC_recuperacion datos = new ADREC_recuperacion(CadenaConexion);
            
            var result = await datos.Obtener(obj);
            return Ok(result);

        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerDetalle(RC_mdl_view obj)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADREC_recuperacion datos = new ADREC_recuperacion(CadenaConexion);

            var result = await datos.ObtenerDetalle(obj);
            return Ok(result);

        }
    }
}
