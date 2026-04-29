using HD.Security;
using HD_Cobranza.Modelos.AgregarContacto;
using HD_Mensajeria.Consultas;
using Microsoft.AspNetCore.Mvc;
using Postventa.Consultas.Dashboard;

namespace HD.Endpoints.Controllers.Mensajeria
{
    public class MensajeriaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public MensajeriaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> obtenerMensajes(string numeroTelefono)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Chat_Mensajes datos = new AD_Obtener_Chat_Mensajes(CadenaConexion);
            var result = await datos.obtenerChat(numeroTelefono);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> apartarChat(string numeroTelefono)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Apartar_Chat datos = new AD_Apartar_Chat(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.Guardar(numeroTelefono, usuario);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> eliminarApartado(string numeroTelefono)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Apartar_Chat datos = new AD_Apartar_Chat(CadenaConexion);
            var usuario = Sesion.usuario();
            var result = await datos.Eliminar(numeroTelefono, usuario);
            return Ok(result);
        }
    }
}
