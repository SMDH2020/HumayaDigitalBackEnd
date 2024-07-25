using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Notifications.Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.Mhusa
{
    public class AC_Validar_Condiciones_OperacionController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public AC_Validar_Condiciones_OperacionController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ValidarCondicionesOperacion(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisiCreditoMhusa datos = new ADAnalisiCreditoMhusa(CadenaConexion);
            var result = await datos.ValidarCondicionesOperacion(folio, Sesion.usuario());
            return Ok(result);

        }

        [Route("/api/[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult> EnviarComentario(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisiCreditoMhusa datos = new ADAnalisiCreditoMhusa(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarValidacion(mdl);
            if (result.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            await NotificacionComentarios.Enviar_Mhusa(result);

            var response = new mdlAnalisis_Mhusa_Resultado
            {
                estado = result.estado,
                socket = result.mdlSolicitud
            };
            return Ok(response);
            
            //ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            //var body = await notificacion.GetBody(mdl);
            //await NotificacionComentarios.Enviar(body);
            //return Ok(result);
        }
    }
}
