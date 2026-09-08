using HD.Security;
using HD_GestionProyectosTI.Consultas;
using HD_GestionProyectosTI.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionProyectosTI
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolTIController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public RolTIController(IConfiguration configuracion, ISesion sesion)
        {
            Configuracion = configuracion;
            Sesion = sesion;
        }

        private string CadenaConexion => Configuracion["ConnectionStrings:GestionProyectosTI"]!;

        // El frontend llama esto al cargar la sesión para saber qué mostrar:
        // formulario de solicitud (todos), bandeja de developer, o panel admin.
        [HttpGet("Mio")]
        public async Task<ActionResult> Mio()
        {
            int idusuario = int.Parse(Sesion.usuario());
            string rol = await new AD_RolTI(CadenaConexion).Obtener(idusuario);
            return Ok(new { idusuario, rol });
        }

        [HttpGet("Listado")]
        public async Task<ActionResult> Listado()
        {
            int idusuario = int.Parse(Sesion.usuario());
            string rol = await new AD_RolTI(CadenaConexion).Obtener(idusuario);
            if (rol != "Admin")
                return Forbid();

            var result = await new AD_RolTI(CadenaConexion).Listado();
            return Ok(result);
        }

        [HttpPost("Asignar")]
        public async Task<ActionResult> Asignar(mdl_RolTI_Asignar mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            string rol = await new AD_RolTI(CadenaConexion).Obtener(idusuario);
            if (rol != "Admin")
                return Forbid();

            await new AD_RolTI(CadenaConexion).Asignar(mdl, idusuario);
            return Ok(new { mensaje = "Rol asignado" });
        }
    }
}
