using DocumentFormat.OpenXml.Office2013.Excel;
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
            string folio = null;
            string usuario = Sesion.usuario();

            AD_Credito_Condicionado_Solicitud_Guardar ac = new AD_Credito_Condicionado_Solicitud_Guardar(CadenaConexion);
            var result = await ac.Guardar(mdl,usuario);
            //foreach (mdl_fecha_compromiso_documentos detalle in mdl.detalle) {
            //    AD_Credito_Condicionado_Fecha_Comprimiso_Documentacion_Vendedor_Guardar datos = new AD_Credito_Condicionado_Fecha_Comprimiso_Documentacion_Vendedor_Guardar(CadenaConexion);
            //    detalle.usuario = Sesion.usuario();
            //    if (detalle.enviar_revision == false) {
            //        detalle.fecha_compromiso = new DateTime(1900, 1, 1);
            //        detalle.comentarios = "No se entregara documento";
            //    }
            //    else if (detalle.fecha_compromiso != DateTime.MinValue)
            //    {

            //        detalle.comentarios = "Fecha de entrega del documento: " + detalle.fecha_compromiso.ToString("dd-MM-yyyy");
            //    }
            //    else
            //    {
            //        detalle.fecha_compromiso = DateTime.Now.Date;
            //        detalle.comentarios = "Documento Entregado";
            //    }
            //    await datos.Guardar(detalle);

            //    // Extrae el folio del primer elemento
            //    if (folio == null)
            //    {
            //        folio = detalle.folio; 
            //    }
            //}
            //AD_Credito_Condicionado_Fecha_Compromiso_Guardar compromiso = new AD_Credito_Condicionado_Fecha_Compromiso_Guardar(CadenaConexion);
            //var result =await compromiso.Guardar(folio,usuario);

            if (result.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            await NotificacionComentarios.EnviarOperacionCondicionada(result);
            return Ok(new { mensaje="Datos cargados con exito"});

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
