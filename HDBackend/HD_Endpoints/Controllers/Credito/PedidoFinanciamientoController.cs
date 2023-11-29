using HD.Clientes.Consultas.PedidoFinanciamiento;
using HD.Clientes.Consultas.PedidoUnidades;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class PedidoFinanciamientoController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PedidoFinanciamientoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlPedido_Detalle_Financiamiento mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_PedidoFinanciamiento_Guardar datos = new AD_PedidoFinanciamiento_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            //AD_ClientesDatosPersonaFisica_Guardar datosfisica = new AD_ClientesDatosPersonaFisica_Guardar(CadenaConexion);
            //await datosfisica.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_PedidoFinanciamiento_Listado datos = new AD_PedidoFinanciamiento_Listado(CadenaConexion);
            var result = await datos.Get(folio);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetByRegistro(string folio, int docto)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_PedidoFinanciamiento_Docto datos = new AD_PedidoFinanciamiento_Docto(CadenaConexion);
            var result = await datos.Get(folio, docto);
            return Ok(result);

        }
    }
}
