using HD_Cobranza.Capturas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.ConvenioPago;
using HD_Cobranza.Capturas.ConvenioPago;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class ClientesDatosContactoGuardarController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesDatosContactoGuardarController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdlClientes_Datos_Contacto_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Datos_Contacto_Guardar datos = new AD_Clientes_Datos_Contacto_Guardar(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }

        private ActionResult Ok(IEnumerable<mdlClientes_Responsable_Cobranza> result)
        {
            throw new NotImplementedException();
        }
    }
}
