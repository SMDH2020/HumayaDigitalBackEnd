using HD_Cobranza.Capturas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Cobranza.Modelos;
using HD_Cobranza.Modelos.ConvenioPago;
using HD_Cobranza.Capturas.ConvenioPago;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class ClientesResponsableCobranzaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesResponsableCobranzaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdlClientes_Responsable_Cobranza mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Responsable_Cobranza datos = new AD_Clientes_Responsable_Cobranza(CadenaConexion);
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
