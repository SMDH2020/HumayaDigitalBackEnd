using HD.Security;
using HD_Ventas.Consultas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Ventas
{
    public class SimuladorIncentivosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SimuladorIncentivosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener_Roles()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Simulador_Incentivos datos = new AD_Simulador_Incentivos(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            //usuario = 5630;
            var result = await datos.Obtener_Roles_Usuario(usuario);
            return Ok(result);
        }
    }
}
