using HD_Buro.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Buro.Modelos;
using HD_Ventas.Consultas;
using HD_Ventas.Modelos;

namespace HD.Endpoints.Controllers.BuroCredito
{
    public class GuardaClientesNoReportadosController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public GuardaClientesNoReportadosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Guardar(mdlGuarda_Clientes_NoReportados mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Guarda_Clientes_NoReportados datos = new AD_Guarda_Clientes_NoReportados(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }
    }
}
