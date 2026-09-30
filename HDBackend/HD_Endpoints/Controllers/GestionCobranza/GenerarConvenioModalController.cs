using HD.Security;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.GestionCobranza.Modelos;
using HD_Reporteria;
using HD_Reporteria.GestionCobranza;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionCobranza
{
    public class GenerarConvenioModalController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public GenerarConvenioModalController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> DropDownListResponsables()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Responsables_Cobranza_DropDownList datos = new AD_Responsables_Cobranza_DropDownList(CadenaConexion);
            var result = await datos.DropDownList();
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdl_Convenio_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Guardar_Convenio_Detalle datos = new AD_Guardar_Convenio_Detalle(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            try
            {
                RPT_Result documento = RPT_Convenio_Guardado.Generar(mdl, result);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                return BadRequest("Error de servidor");

            }
        }
    }
}
