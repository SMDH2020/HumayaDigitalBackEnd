using HD.Security;
using HD_GestionProyectosTI.Consultas.Roles;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionProyectosTI
{
    public class RolesTIController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public RolesTIController(IConfiguration configuracion, ISesion sesion)
        {
            Configuracion = configuracion;
            Sesion = sesion;
        }

        private string CadenaConexion => Configuracion["ConnectionStrings:GestionProyectosTI"]!;

        // Roles activos del usuario autenticado dentro del modulo. Un usuario
        // sin filas en TI_UsuarioRoles no tiene ningun rol especial: se trata
        // como solicitante basico en el resto de los endpoints.
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Mio()
        {
            int idusuario = int.Parse(Sesion.usuario());
            AD_TI_Roles_Mio datos = new AD_TI_Roles_Mio(CadenaConexion);
            var result = await datos.Mio(idusuario);
            return Ok(result);
        }
    }
}
