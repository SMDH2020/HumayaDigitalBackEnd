using HD.Security;
using HD_Cobranza.Capturas;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class PermisoCobranzaController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public PermisoCobranzaController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetPermiso()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Permiso_Cobranza datos = new AD_Permiso_Cobranza(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.Obtener(usuario);
            return Ok(result);
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetSucursales()
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Permiso_Cobranza datos = new AD_Permiso_Cobranza(CadenaConexion);
            int usuario = int.Parse(Sesion.usuario());
            var result = await datos.GetSucursales(usuario);
            return Ok(result);
        }
    }
}
