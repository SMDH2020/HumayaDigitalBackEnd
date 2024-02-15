using HD.Security;
using HD_Finanzas.AccesoDatos.Actions;
using HD_Finanzas.Modelos.Estado_Resultados;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FEstadoResultadosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FEstadoResultadosController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetEstadoResultadosByDireccionRolado(Fmdl_EstadoResultadosRolado prm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_EstadoResultados estadoresultados = new FAD_EstadoResultados(CadenaConexion);
            var result = await estadoresultados.GetEstadoResultadosByDireccionRolado(prm,"1");
            return Ok(result);
        }
    }
}
