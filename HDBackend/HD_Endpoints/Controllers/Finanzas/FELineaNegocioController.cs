using HD.Security;
using HD_Finanzas.AccesoDatos;
using HD_Finanzas.Modelos.Linea_Negocio;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Finanzas
{
    public class FELineaNegocioController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public FELineaNegocioController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetEsquemaByLineadeNegocio(Fmdl_Linea_negocio_filtros vm)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            FAD_LineaNegocio datos = new FAD_LineaNegocio(CadenaConexion);
            var result = await datos.GetEsquemaByLineadeNegocio(vm);
            return Ok(result);
        }
    }

}
