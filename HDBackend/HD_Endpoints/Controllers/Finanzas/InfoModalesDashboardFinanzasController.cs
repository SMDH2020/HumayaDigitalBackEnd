using HD_Finanzas.AccesoDatos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;
using HD_Finanzas.AccesoDatos.Tasa_de_intereses;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class InfoModalesDashboardFinanzasController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public InfoModalesDashboardFinanzasController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> Obtener_FactorAbsorcionModal()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_InfoModalFactorAbsorcion datos = new AD_InfoModalFactorAbsorcion(CadenaConexion);
            var result = await datos.GetFactorAbsorcion();
            return Ok(result);
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> Obtener_RotacionActivosModal()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_InfoModalRotacionActivos datos = new AD_InfoModalRotacionActivos(CadenaConexion);
            var result = await datos.GetRotacionActivos();
            return Ok(result);
        }
    }
}
