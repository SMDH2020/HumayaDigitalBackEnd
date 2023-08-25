using HD.Clientes.Consultas.GiroEmpresarial;
using HD.Clientes.Consultas.SolicitudCreditoSiniestros;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class SolicitudCreditoSiniestrosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SolicitudCreditoSiniestrosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlSolicitud_Credito_Siniestros mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_SolicitudCreditoSiniestros_Guardar datos = new AD_SolicitudCreditoSiniestros_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito" });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> Listado(short filtrar)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_SolicitudCreditoSiniestros_Listado datos = new AD_SolicitudCreditoSiniestros_Listado(CadenaConexion);
            var result = await datos.Listado(filtrar);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{id}")]
        public async Task<ActionResult> BuscarID(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_SolicitudCreditoSiniestros_BuscarID datos = new AD_SolicitudCreditoSiniestros_BuscarID(CadenaConexion);
            var result = await datos.BuscarID(folio);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownList()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Login"];
            AD_SolicitudCreditoSiniestros_DropDownList datos = new AD_SolicitudCreditoSiniestros_DropDownList(CadenaConexion);
            var result = await datos.DropDownList();
            return Ok(result);

        }
    }
}
