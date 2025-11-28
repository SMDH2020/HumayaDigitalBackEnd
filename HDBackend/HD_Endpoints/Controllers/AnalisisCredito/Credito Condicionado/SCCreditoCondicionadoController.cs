using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Consultas.Credito_Condicionado;
using HD.Clientes.Modelos.SC_Analisis;
using HD.Clientes.Modelos;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using HD.Notifications.Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD.Notifications.Consultas;
using System.Globalization;

namespace HD.Endpoints.Controllers.AnalisisCredito.Credito_Condicionado
{
    public class SCCreditoCondicionadoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SCCreditoCondicionadoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        //[HttpPost]
        //public async Task<ActionResult> Enviar(mdlSCCredito_Condicionado mdl)
        //{
        //    string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
        //    AD_Credito_Condicionado_Enviar datos = new AD_Credito_Condicionado_Enviar(CadenaConexion);
        //    mdl.usuario = Sesion.usuario();
        //    var result = await datos.BuscarFolio(mdl);
        //    return Ok(result);

        //}

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CrearCondicionado(mdlSCCredito_Condicionado mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Credito_Condicionado_Enviar datos = new AD_Credito_Condicionado_Enviar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.BuscarFolio(mdl);

            //enviar notificacion
            var usuariosNotificados = string.Join(",", result.mdlSolicitud?.Select(u => u.idempleado.ToString()) ?? new List<string>());
            var usuario = Sesion.usuario();
            var textoCliente = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mdl.cliente.ToLower());

            //AD_Conseguir_Mensaje_Manual usuarios = new AD_Conseguir_Mensaje_Manual(CadenaConexion);
            //var resultado = await usuarios.GuardarNotificacionSolicitud(result.estado.folio_condicionado, "Se activo crédito condicionado para " + textoCliente, 9, usuario, usuariosNotificados);

            //AD_HD_Notificaciones_Enviar_Push notificacionPush = new AD_HD_Notificaciones_Enviar_Push(CadenaConexion);
            //await notificacionPush.Enviar_Notificacion_Solicitud(resultado, "Humaya Digital");

            return Ok(result);

        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CancelarCondicionado(mdlSCCredito_Condicionado mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Credito_Condicionado_Enviar datos = new AD_Credito_Condicionado_Enviar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Cancelar(mdl);
            return Ok(new { socket = result } );
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> FinalizaCreditoCondicionado(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Finaliza_Credito_Condicionado_Guardar datos = new AD_Finaliza_Credito_Condicionado_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            if (result is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            else
            {
                await NotificacionComentarios.Enviar_Mhusa(result);

            }
            var response = new mdlAnalisis_Mhusa_Resultado
            {
                estado = result.estado,
                socket = result.mdlSolicitud
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> FinalizaCreditoJDFCondicionado(mdlSCAnalisis_Comentarios mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Finaliza_Credito_Condicionado_Guardar datos = new AD_Finaliza_Credito_Condicionado_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.GuardarJDFCondicionado(mdl);
            if (result is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            else
            {
                await NotificacionComentarios.Enviar_Mhusa(result);

            }
            var response = new mdlAnalisis_Mhusa_Resultado
            {
                estado = result.estado,
                socket = result.mdlSolicitud
            };
            return Ok(response);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Cancelar(mdlSCCredito_Condicionado mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Credito_Condicionado_Cancelar datos = new AD_Credito_Condicionado_Cancelar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.CancelarFolio(mdl);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CargarTimelineCondicionado(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Cargar_Credito_Condicionado datos = new AD_Cargar_Credito_Condicionado(CadenaConexion);
            string usuario = Sesion.usuario();
            var result = await datos.BuscarFolio(folio, usuario);
            return Ok(result);

        }
    }
}
