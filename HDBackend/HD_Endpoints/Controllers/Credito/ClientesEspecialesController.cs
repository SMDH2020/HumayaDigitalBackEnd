using HD.Clientes.Consultas.ClientesDocumentacion;
using HD.Clientes.Consultas.Especiales;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.Especiales;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ClientesEspecialesController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesEspecialesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Post(mdlClientesEspeciales mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADClientesEspeciales datos = new ADClientesEspeciales(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito", listado = result });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADClientesEspeciales datos = new ADClientesEspeciales(CadenaConexion);
            var result = await datos.Listado();
            return Ok(result);

        }
    }
}
