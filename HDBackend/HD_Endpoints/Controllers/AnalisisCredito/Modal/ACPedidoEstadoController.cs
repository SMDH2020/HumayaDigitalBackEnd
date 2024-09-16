using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Consultas.AnalisisCredito.Modal;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.Modal
{
    public class ACPedidoEstadoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACPedidoEstadoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Get(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Pedido_Estado datos = new ADAnalisis_Pedido_Estado(CadenaConexion);
            var result = await datos.Get(folio, Sesion.usuario());
            return Ok(result);

        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Comentarios datos = new ADAnalisis_Comentarios(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);

            //ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            return Ok(result);
        }
    }
}
