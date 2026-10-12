using HD_Buro.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.BuroCredito
{
    public class CargaReporteClientesNoReportadosController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public CargaReporteClientesNoReportadosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CargarNoReportados()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Carga_Clientes_NoReportados datos = new AD_Carga_Clientes_NoReportados(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.reporte(usuario);
            return Ok(result);
        }
    }
}
