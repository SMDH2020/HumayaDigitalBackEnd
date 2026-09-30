using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.GestionCobranza.Modelos;
using HD_Reporteria;
using HD_Reporteria.GestionCobranza;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionCobranza
{
    public class GuardarGestionCobranzaComentariosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public GuardarGestionCobranzaComentariosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_Gestion_Cobranza_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Guarda_Gestion_Cobranza_Comentarios datos = new AD_Guarda_Gestion_Cobranza_Comentarios(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }
    }
}
