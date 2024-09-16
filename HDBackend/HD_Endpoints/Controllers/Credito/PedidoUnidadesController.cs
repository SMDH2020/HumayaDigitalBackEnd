using HD.Clientes.Consultas.PedidoDatosGenerales;
using HD.Clientes.Consultas.PedidoUnidades;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class PedidoUnidadesController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PedidoUnidadesController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlPedido_Unidades mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_PedidoUnidades_Guardar datos = new AD_PedidoUnidades_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_PedidoUnidades_Listado datos = new AD_PedidoUnidades_Listado(CadenaConexion);
            var result = await datos.Get(folio);
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Delete(string folio,int registro)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_PedidoUnidades_DeleteRow datos = new AD_PedidoUnidades_DeleteRow(CadenaConexion);

            var result = await datos.Delete(folio,registro,Sesion.usuario());
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetByRegistro(string folio, int registro)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_PedidoUnidades_ByRegistro datos = new AD_PedidoUnidades_ByRegistro(CadenaConexion);
            var result = await datos.Get(folio, registro);
            return Ok(result);

        }
    }
}
