using HD_Finanzas.AccesoDatos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Finanzas.Modelos;
using HD_Finanzas.Modelos.Balance_General;
using Enlace.Dapper.Reportes;
using HD_Finanzas.AccesoDatos.BalanceGeneral.Complementos;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FBalanceGeneralController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FBalanceGeneralController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BalanceGeneral(vmBalanceGeneral vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_BalanceGeneral bg = new AD_BalanceGeneral(CadenaConexion);
            return Ok(await bg.GetBalanceGeneral(vm));
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ExcelBalanceGeneral(vmBalanceGeneral vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_BalanceGeneral bg = new AD_BalanceGeneral(CadenaConexion);
            var result = await bg.GetBalanceGeneral(vm);
            return Ok(await DocBalanceGeneral.CrearExel(result.balance));
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BalanceGeneralConsolidadoExcel(vmBalanceGeneral vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_BalanceGeneral bg = new AD_BalanceGeneral(CadenaConexion);
            var result = await bg.GetBalanceConsolidado(vm);
            return Ok(await DocBalanceGeneralConsolidado.CrearExcel(result, vm));
        }
    }
}
