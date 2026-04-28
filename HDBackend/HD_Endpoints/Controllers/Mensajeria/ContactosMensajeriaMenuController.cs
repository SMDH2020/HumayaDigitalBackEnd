using HD.Security;
using HD_Cobranza.Modelos.AgregarContacto;
using HD_Mensajeria.Consultas;
using Microsoft.AspNetCore.Mvc;
using Ventas.Consultas.CotizacionesVentas;

namespace HD.Endpoints.Controllers.Mensajeria
{
    public class ContactosMensajeriaMenuController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ContactosMensajeriaMenuController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> obtenerContactos()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Obtener_Listado_Contactos_Mensajeria_Menu datos = new AD_Obtener_Listado_Contactos_Mensajeria_Menu(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.obtenerContactos(usuario);
            return Ok(result);
        }
    }
}
