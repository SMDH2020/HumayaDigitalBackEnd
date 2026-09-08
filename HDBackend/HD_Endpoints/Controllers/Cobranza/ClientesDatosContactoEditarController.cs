using HD_Cobranza.Capturas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.ConvenioPago;
using HD_Cobranza.Capturas.ConvenioPago;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class ClientesDatosContactoEditarController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesDatosContactoEditarController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Editar(mdlClientes_Datos_Contacto_Editar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Datos_Contacto_Editar datos = new AD_Clientes_Datos_Contacto_Editar(CadenaConexion);
            mdl.usuario = int.Parse(Sesion.usuario());
            var result = await datos.Editar(mdl);
            return Ok(result);
        }

        private ActionResult Ok(IEnumerable<mdlClientes_Responsable_Cobranza> result)
        {
            throw new NotImplementedException();
        }
    }
}
