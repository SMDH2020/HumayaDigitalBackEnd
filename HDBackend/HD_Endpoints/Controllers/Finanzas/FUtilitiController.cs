using HD.Security;
using HD_Finanzas.AccesoDatos.Actions;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FUtilitiController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FUtilitiController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetAdrSucursalDepartamento(string defaultuser)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_AdrSucursalDepto utilities = new(CadenaConexion);
            var result = await utilities.GetASD("1");
            return Ok(result);
        }
    }
}
