using HD.Clientes.Consultas.Credito_Condicionado;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using HD.Notifications.Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.Credito_Condicionado
{
    public class SCFacturacionCondicionadaController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SCFacturacionCondicionadaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Enviar(mdl_Autorizar_facturacion_View mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            mdl.usuario = Sesion.usuario();
            AD_Credito_Autorizar_Facturacion_Condicionada da = new AD_Credito_Autorizar_Facturacion_Condicionada(CadenaConexion);
            var result = await da.Guardar(mdl);

            AD_Credito_Condicionado_Notificacion_Correo correo = new AD_Credito_Condicionado_Notificacion_Correo(CadenaConexion);
            var resultado  = await correo.Notificacion(mdl.folio,mdl.usuario,mdl.comentarios, 150);

            if (resultado.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            await NotificacionComentarios.EnviarNotificacionOperacionCondicionada(resultado);
            return Ok(result);

        }
    }
}
