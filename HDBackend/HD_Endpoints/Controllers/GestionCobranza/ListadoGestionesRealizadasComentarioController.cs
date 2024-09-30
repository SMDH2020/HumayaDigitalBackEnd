using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.GestionCobranza.Modelos;
using HD_Reporteria.GestionCobranza;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionCobranza
{
    public class ListadoGestionesRealizadasComentarioController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ListadoGestionesRealizadasComentarioController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ListadoParametros(int ejercicio, int periodo, string adr, string sucursal, int responsable)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Listado_Gestiones_Realizadas_Comentario datos = new AD_Listado_Gestiones_Realizadas_Comentario(CadenaConexion);
            var result = await datos.Get(ejercicio, periodo, adr, sucursal, responsable);
            return Ok(result);
        }
    }
}
