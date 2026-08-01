using HD.Security;
using HD_GestionProyectosTI.Consultas;
using HD_GestionProyectosTI.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionProyectosTI
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActividadesTIController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public ActividadesTIController(IConfiguration configuracion, ISesion sesion)
        {
            Configuracion = configuracion;
            Sesion = sesion;
        }

        private string CadenaConexion => Configuracion["ConnectionStrings:GestionProyectosTI"]!;

        // Solo Admin desglosa actividades (etapa "En definición").
        [HttpPost("Crear")]
        public async Task<ActionResult> Crear(mdl_ActividadCrear mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            string rol = await new AD_RolTI(CadenaConexion).Obtener(idusuario);
            if (rol != "Admin")
                return Forbid();

            int idactividad = await new AD_Actividades(CadenaConexion).Crear(mdl, idusuario);
            return Ok(new { idactividad, mensaje = "Actividad creada" });
        }

        [HttpGet("Listado/{idsolicitud}")]
        public async Task<ActionResult> Listado(int idsolicitud)
        {
            var result = await new AD_Actividades(CadenaConexion).Listado(idsolicitud);
            return Ok(result);
        }

        // Vista del developer: solo lo que tiene asignado.
        [HttpGet("Mias")]
        public async Task<ActionResult> Mias()
        {
            int idusuario = int.Parse(Sesion.usuario());
            var result = await new AD_Actividades(CadenaConexion).ListadoPorDeveloper(idusuario);
            return Ok(result);
        }

        // El developer solo puede mover el estado de SUS actividades. El
        // Admin puede mover cualquiera.
        [HttpPost("MarcarEstado")]
        public async Task<ActionResult> MarcarEstado(mdl_MarcarEstadoActividad mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            string rol = await new AD_RolTI(CadenaConexion).Obtener(idusuario);

            if (rol != "Admin")
            {
                var dueno = await new AD_Actividades(CadenaConexion).ObtenerDeveloperDeActividad(mdl.idactividad);
                if (dueno == null)
                    return NotFound(new { mensaje = "Actividad no encontrada" });
                if (dueno != idusuario)
                    return Forbid();
            }

            await new AD_Actividades(CadenaConexion).MarcarEstado(mdl, idusuario);
            return Ok(new { mensaje = "Estado de actividad actualizado" });
        }
    }
}
