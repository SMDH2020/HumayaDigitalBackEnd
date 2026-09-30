using HD.Clientes.Consultas.AnalisisCredito.Modal;
using HD.Clientes.Modelos.SC_Analisis.Modal;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.Modal
{
    public class ADAnalisisReporteCampoController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ADAnalisisReporteCampoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Get(mdlSCAnalisis_Dedidion_View mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_ReporteCampo datos = new ADAnalisis_ReporteCampo(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Get(mdl);
            return Ok(result);

        }
    }
}
