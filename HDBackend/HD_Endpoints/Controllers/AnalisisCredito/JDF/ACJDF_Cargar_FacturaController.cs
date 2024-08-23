using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Consultas.AnalisisCredito.JDF;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Notifications.Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

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
            foreach (mdl_documentos_facturados_EQUIP fac in mdl.documentos)
            {
                await datos.Guardar_detalle(mdl.folio, mdl.registro, fac.orden, fac.documento, fac.documento, fac.docto_financiamiento);
            }
            return Ok(result);
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
            //ADNotificacionFinalizacionProceso notificacion = new ADNotificacionFinalizacionProceso(CadenaConexion);
            //var body = await notificacion.GetBody(mdl.folio);
            //await NotificacionComentarios.EnviarProcesoFinalizado(body, mdl.folio);
            return Ok(new {mensaje="Datos Cargados cone exito"});
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
