using HD.Clientes.Consultas.SolicitudCredito;
using HD.Clientes.Modelos;
using HD.Clientes.Notificaciones;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class SolicitudCreditoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SolicitudCreditoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlSolicitud_Credito mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_SolicitudCredito_Guardar datos = new AD_SolicitudCredito_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{idCliente}")]
        public async Task<ActionResult> Listado(short idCliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_SolicitudCredito_Listado datos = new AD_SolicitudCredito_Listado(CadenaConexion);
            var result = await datos.Listado(idCliente);
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> detalle(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_SolicitudCredito_Detalle datos = new AD_SolicitudCredito_Detalle(CadenaConexion);
            var result = await datos.Detalle(folio,Sesion.usuario());
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> enviar(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_SolcitudCredito_Enviar datos = new AD_SolcitudCredito_Enviar(CadenaConexion);
            var result = await datos.Detalle(folio, Sesion.usuario());
            string mensaje = "Solicitud enviada a revision";
            return Ok(new { mensaje });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> BuscarID(string id)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_SolicitudCredito_BuscarID datos = new AD_SolicitudCredito_BuscarID(CadenaConexion);
            var result = await datos.BuscarID(id);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownList()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_SolicitudCredito_DropDownList datos = new AD_SolicitudCredito_DropDownList(CadenaConexion);
            var result = await datos.DropDownList();
            return Ok(result);

        }
    }
}
