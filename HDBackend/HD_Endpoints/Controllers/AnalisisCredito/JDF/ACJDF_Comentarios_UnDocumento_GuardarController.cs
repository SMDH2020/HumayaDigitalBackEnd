using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Consultas.AnalisisCredito.JDF;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Notifications.Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.JDF
{
    public class ACJDF_Comentarios_UnDocumento_GuardarController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACJDF_Comentarios_UnDocumento_GuardarController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Guardar(mdlJDFAnalisiComentarios_Guardar_View mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            mdl.usuario = Sesion.usuario();
            ADJDFAnalisis_Comentarios_Guardar datos = new ADJDFAnalisis_Comentarios_Guardar(CadenaConexion);
            var result = await datos.Guardar(mdl);
            if (result is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            ADAnalisisNotificacion notificacion = new ADAnalisisNotificacion(CadenaConexion);
            mdlSCAnalisis_Comentarios not = new mdlSCAnalisis_Comentarios()
            {
                   folio=mdl.folio,
                   idproceso=mdl.idproceso,
                   estatus=mdl.estatus
            };
            var body = await notificacion.GetBody(not);
            //await NotificacionComentarios.Enviar(body);
            return Ok(result);

        }
    }
}
