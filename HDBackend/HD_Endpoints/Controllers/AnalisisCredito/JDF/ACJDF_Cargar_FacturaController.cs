using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Consultas.AnalisisCredito.JDF;
using HD.Clientes.Consultas.Credito_Condicionado;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Notifications.Analisis;
using HD.Notifications.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace HD.Endpoints.Controllers.AnalisisCredito.JDF
{
    public class ACJDF_Cargar_FacturaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACJDF_Cargar_FacturaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Guardar(mdlJDFAnalisis_Datos_Facturacion_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADJDF_Analisis_Cargar_Factura datos = new ADJDF_Analisis_Cargar_Factura(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            //foreach (mdl_documentos_facturados_EQUIP fac in mdl.documentos)
            //{
            //    await datos.Guardar_detalle(mdl.folio, mdl.registro, fac.orden, fac.documento, mdl.usuario, fac.docto_financiamiento);
            //}

            //enviar notificacion
            var usuariosNotificados = string.Join(",", result.mdlSolicitud?.Select(u => u.idempleado.ToString()) ?? new List<string>());
            var usuario = Sesion.usuario();
            var textoCliente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.documento.cliente.ToLower());
            var idevento = 1;
            var referencia = 9;

            AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
            var resultado = await usuarios.GuardarNotificacionSolicitud(idevento, referencia, "Solicitud facturada de" + textoCliente, mdl.folio, usuariosNotificados);

            AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion);
            await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");

            return Ok(new
            {
                documentacion = result.documento,
                socket = result.mdlSolicitud
            });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarMhusaDetalle(mdlJDFAnalisis_Datos_Facturacion_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADJDF_Analisis_Cargar_Factura datos = new ADJDF_Analisis_Cargar_Factura(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            foreach (mdl_documentos_facturados_EQUIP fac in mdl.documentos)
            {
                await datos.Guardar_detalle(mdl.folio, mdl.registro, fac.orden, fac.documento,mdl.usuario, fac.docto_financiamiento);
            }
            AD_Conseguir_Correos_Notificacion correos = new AD_Conseguir_Correos_Notificacion(CadenaConexion);
            var socket =  await correos.ObtenerCorreos(mdl.folio, mdl.usuario, mdl.comentarios);
            //ADNotificacionFinalizacionProceso notificacion = new ADNotificacionFinalizacionProceso(CadenaConexion);
            //var body = await notificacion.GetBody(mdl.folio);
            //await NotificacionComentarios.EnviarProcesoFinalizado(body, mdl.folio);

            //enviar notificacion
            var usuariosNotificados = string.Join(",", socket.Select(u => u.idempleado.ToString()) ?? new List<string>());
            var usuario = Sesion.usuario();
            //var textoCliente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.mdldatos.cliente.ToLower());
            var idevento = mdl.folio.Substring(0, 2) == "PC" ? 3 : mdl.folio.Substring(0, 2) == "RC" ? 4 : 1;
            var referencia = 9;

            AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
            var resultado = await usuarios.GuardarNotificacionSolicitud(idevento, referencia, "Se cargo pagare equip de la solicitud: " + mdl.folio, mdl.folio, usuariosNotificados);

            AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion);
            await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");
            return Ok(new {socket=socket});
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarFacturaCompleta(mdlJDFAnalisis_Datos_Facturacion_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADJDF_Analisis_Cargar_Factura datos = new ADJDF_Analisis_Cargar_Factura(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            //foreach (mdl_documentos_facturados_EQUIP fac in mdl.documentos)
            //{
            //    await datos.Guardar_detalle(mdl.folio, mdl.registro, fac.orden, fac.documento, mdl.usuario, fac.docto_financiamiento);
            //}
            return Ok(new
            {
                documentacion = result.documento,
                socket = result.mdlSolicitud
            });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarMhusaDetalleCondicionado(mdlJDFAnalisis_Datos_Facturacion_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADJDF_Analisis_Cargar_Factura datos = new ADJDF_Analisis_Cargar_Factura(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            foreach (mdl_documentos_facturados_EQUIP fac in mdl.documentos)
            {
                await datos.Guardar_detalle_Condicionado(mdl.folio, mdl.registro, fac.orden, fac.documento, mdl.usuario, fac.docto_financiamiento);
            }
            AD_Credito_Condicionado_Notificacion_Correo correo = new AD_Credito_Condicionado_Notificacion_Correo(CadenaConexion);
            var resultado = await correo.Notificacion(mdl.folio, mdl.usuario, mdl.comentarios, 250);
            if (resultado.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            await NotificacionComentarios.EnviarNotificacionOperacionCondicionada(resultado);
            return Ok(new { mensaje = "Datos Cargados cone exito" });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarMhusaDetalleCondicionadoMhusa(mdlJDFAnalisis_Datos_Facturacion_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADJDF_Analisis_Cargar_Factura datos = new ADJDF_Analisis_Cargar_Factura(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            foreach (mdl_documentos_facturados_EQUIP fac in mdl.documentos)
            {
                await datos.Guardar_detalle_Condicionado_Mhusa(mdl.folio, mdl.registro, fac.orden, fac.documento, mdl.usuario, fac.docto_financiamiento);
            }
            AD_Credito_Condicionado_Notificacion_Correo correo = new AD_Credito_Condicionado_Notificacion_Correo(CadenaConexion);
            var resultado = await correo.Notificacion(mdl.folio, mdl.usuario, mdl.comentarios, 250);
            if (resultado.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            await NotificacionComentarios.EnviarNotificacionOperacionCondicionada(resultado);
            return Ok(new { socket = resultado.mdlSolicitud });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarMhusa(mdlJDFAnalisis_Datos_Facturacion_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADJDF_Analisis_Cargar_Factura datos = new ADJDF_Analisis_Cargar_Factura(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarMhusa(mdl);
            if (result.mdldatos is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            await NotificacionComentarios.Enviar_Mhusa(result);

            //enviar notificacion
            var usuariosNotificados = string.Join(",", result.mdlSolicitud?.Select(u => u.idempleado.ToString()) ?? new List<string>());
            var usuario = Sesion.usuario();
            var textoCliente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result.mdldatos.cliente.ToLower());
            var idevento = 1;
            var referencia = 9;

            AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
            var resultado = await usuarios.GuardarNotificacionSolicitud(idevento, referencia, "Solicitud facturada de" + textoCliente , mdl.folio, usuariosNotificados);

            AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion);
            await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");

            return Ok(result);
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Get(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADJDF_Analisis_Cargar_Factura datos = new ADJDF_Analisis_Cargar_Factura(CadenaConexion);
            var result = await datos.Obtener(folio, Sesion.usuario());
            return Ok(result);

        }
    }
}
