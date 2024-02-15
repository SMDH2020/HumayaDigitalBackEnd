using HD.Security;
using HD_Cobranza.Capturas;
using HD_Cobranza.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class RegistroClientesController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public RegistroClientesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Registrar(ctlRegistrarCliente obj)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADRegistroClientes datos = new ADRegistroClientes(CadenaConexion);
            obj.usuario = Sesion.usuario();
            var result = await datos.Guardar(obj);
            return Ok(result);

        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Combinar(ctlCombinarCliente obj)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADRegistroClientes datos = new ADRegistroClientes(CadenaConexion);
            obj.usuario = Sesion.usuario();
            var result = await datos.Combinar(obj);
            return Ok(result);

        }
    }
}
