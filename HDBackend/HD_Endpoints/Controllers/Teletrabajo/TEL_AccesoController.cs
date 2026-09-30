using HD.Security;
using Microsoft.AspNetCore.Mvc;
using Teletrabajo.Consultas;
using Teletrabajo.Modelos;

namespace HD.Endpoints.Controllers.Teletrabajo
{
    public class TEL_AccesoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public TEL_AccesoController(IConfiguration configuration,
            ISesion sesion)

        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> acceso(TEL_mdl_InfoSesion mdl)
        {
            mdl.usuario = int.Parse(Sesion.usuario());
            string CadenaConexion = Configuracion["ConnectionStrings:Teletrabajo"];
            TEL_AD_RegistrarSesion datos = new TEL_AD_RegistrarSesion(CadenaConexion);
            var result = await datos.Acceso(mdl);

            return Ok(result);

        }
    }
}
