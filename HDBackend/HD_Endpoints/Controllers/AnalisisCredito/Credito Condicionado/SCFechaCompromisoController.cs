using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Consultas.Credito_Condicionado;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using HD.Notifications.Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.Credito_Condicionado
{
    public class SCFechaCompromisoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SCFechaCompromisoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Enviar(mdl_fecha_compromiso_documentos_detalle mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            foreach (mdl_fecha_compromiso_documentos detalle in mdl.detalle) {
                AD_Credito_Condicionado_Fecha_Compromiso_Documentos datos = new AD_Credito_Condicionado_Fecha_Compromiso_Documentos(CadenaConexion);
                detalle.usuario = Sesion.usuario();
                if (detalle.enviar_revision == false) {
                    detalle.comentarios = "No se entregara documento";
                }
                else if (detalle.fecha_compromiso != DateTime.MinValue)
                {
                    detalle.comentarios = "Fecha de entrega del documento: " + detalle.fecha_compromiso.ToString();
                }
                else
                {
                    detalle.comentarios = "Documento Entregado";
                }
                var result = await datos.Guardar(detalle);
            }


            //if (result.mdldatos is null)
            //{
            //    return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            //}
            //await NotificacionComentarios.EnviarOperacionCondicionada(result);
            return Ok("hola");

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Credito_Condicionado_Fecha_Compromiso_Obtener datos = new AD_Credito_Condicionado_Fecha_Compromiso_Obtener(CadenaConexion);
            var result = await datos.Get(folio);
            return Ok(result);

        }
    }
}
