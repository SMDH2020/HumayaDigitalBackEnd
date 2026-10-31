using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.GestionCobranza.Modelos;
using HD_Reporteria;
using HD_Reporteria.GestionCobranza;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionCobranza
{
    public class GuardarReestructuracionGestionController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public GuardarReestructuracionGestionController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_Gestion_Cobranza_Reestructuracion mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Guarda_Gestion_Cobranza_Reestructuracion datos = new AD_Guarda_Gestion_Cobranza_Reestructuracion(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            string email = Configuracion["Correo:Email"];
            string password = Configuracion["Correo:Password"];
            var result = await datos.Guardar(mdl, email, password);
            return Ok(result);
        }
    }
}
