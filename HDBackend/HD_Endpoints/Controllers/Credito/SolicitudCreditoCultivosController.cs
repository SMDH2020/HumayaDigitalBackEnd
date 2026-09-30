using HD.Clientes.Consultas.SolicitudCreditoCultivos;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class SolicitudCreditoCultivosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SolicitudCreditoCultivosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlSolicitud_Credito_Cultivos mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_SolicitudCreditoCultivos_Guardar datos = new AD_SolicitudCreditoCultivos_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{folio}")]
        public async Task<ActionResult> Listado(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_SolicitudCreditoCultivos_Listado datos = new AD_SolicitudCreditoCultivos_Listado(CadenaConexion);
            var result = await datos.Listado(folio);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> BuscarID(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_SolicitudCreditoCultivos_BuscarID datos = new AD_SolicitudCreditoCultivos_BuscarID(CadenaConexion);
            var result = await datos.BuscarID(folio);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownList()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_SolicitudCreditoCultivos_DropDownList datos = new AD_SolicitudCreditoCultivos_DropDownList(CadenaConexion);
            var result = await datos.DropDownList();
            return Ok(result);

        }
    }
}
