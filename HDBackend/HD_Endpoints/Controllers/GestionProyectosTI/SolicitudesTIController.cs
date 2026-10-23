using HD.Notifications.GestionProyectosTI;
using HD.Notifications.Modelos;
using HD.Security;
using HD_GestionProyectosTI.Consultas.Roles;
using HD_GestionProyectosTI.Consultas.Solicitudes;
using HD_GestionProyectosTI.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionProyectosTI
{
    public class SolicitudesTIController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public SolicitudesTIController(IConfiguration configuracion, ISesion sesion)
        {
            Configuracion = configuracion;
            Sesion = sesion;
        }

        private string CadenaConexion => Configuracion["ConnectionStrings:GestionProyectosTI"]!;
        private mdl_Correo_M365 ConfigCorreo => Configuracion.GetSection("CorreoM365").Get<mdl_Correo_M365>()!;

        // Roles con visibilidad global sobre las solicitudes (ven todo el
        // listado, pueden abrir cualquier detalle). El resto (DESARROLLADOR o
        // sin rol asignado) solo ve lo que solicito o donde participa.
        private static readonly string[] RolesVisibilidadGlobal = { "JEFATURA_TI", "GERENCIA_PLANEACION", "ADMIN_MODULO" };

        // Roles autorizados para mover el estado de una solicitud en esta
        // fase (registro/revision). Priorizacion, estimacion y asignacion
        // de desarrollo quedan para fases posteriores y no se habilitan aqui.
        private static readonly string[] RolesCambioEstado = { "JEFATURA_TI", "ADMIN_MODULO" };

        private async Task<bool> TieneVisibilidadGlobalAsync(int idusuario)
        {
            var roles = await new HD_GestionProyectosTI.Consultas.Roles.AD_TI_Roles_Mio(CadenaConexion).Mio(idusuario);
            return roles.Any(r => RolesVisibilidadGlobal.Contains(r.Codigo));
        }

        private async Task<bool> PuedeCambiarEstadoAsync(int idusuario)
        {
            var roles = await new HD_GestionProyectosTI.Consultas.Roles.AD_TI_Roles_Mio(CadenaConexion).Mio(idusuario);
            return roles.Any(r => RolesCambioEstado.Contains(r.Codigo));
        }

        // Eliminar una solicitud (sin importar su estatus) queda reservado
        // solo a Jefatura de TI -- a diferencia de PuedeCambiarEstadoAsync,
        // aqui no se incluye ADMIN_MODULO.
        private async Task<bool> EsJefaturaTIAsync(int idusuario)
        {
            var roles = await new HD_GestionProyectosTI.Consultas.Roles.AD_TI_Roles_Mio(CadenaConexion).Mio(idusuario);
            return roles.Any(r => r.Codigo == "JEFATURA_TI");
        }

        // Dueño de la solicitud (la registro) o participante activo
        // (TI_SolicitudParticipantes). Centraliza la regla de acceso al
        // detalle/historial para no repetirla en cada accion.
        private async Task<bool> TieneAccesoAsync(mdl_TI_Solicitud solicitud, int idusuario)
        {
            if (solicitud.SolicitanteUsuarioId == idusuario || solicitud.CreadaPorUsuarioId == idusuario)
                return true;

            if (await TieneVisibilidadGlobalAsync(idusuario))
                return true;

            return await new AD_TI_Solicitudes_EsParticipante(CadenaConexion).EsParticipante(solicitud.SolicitudId, idusuario);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Crear(mdl_TI_SolicitudCrear mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            mdl.SolicitanteUsuarioId = idusuario;
            mdl.CreadaPorUsuarioId = idusuario;

            AD_TI_Solicitudes_Crear datos = new AD_TI_Solicitudes_Crear(CadenaConexion);
            var (solicitudId, folio) = await datos.Crear(mdl);

            // Aviso a Jefatura de TI de que hay una solicitud nueva por
            // revisar. El solicitante ya se conoce (es quien hizo la
            // peticion), solo falta el nombre -- se resuelve por separado
            // porque Crear() solo regresa solicitudId/folio.
            var jefaturaTI = await new AD_TI_Roles_UsuariosPorRol(CadenaConexion).Listado("JEFATURA_TI");
            var correosJefatura = jefaturaTI.Select(u => u.Correo).Where(c => !string.IsNullOrWhiteSpace(c)).Select(c => c!).ToList();
            var solicitud = await new AD_TI_Solicitudes_Obtener(CadenaConexion).Obtener(solicitudId);
            await NotificacionTI.EnviarSolicitudCreada(ConfigCorreo, folio, mdl.Titulo, solicitud?.SolicitanteNombre ?? "", correosJefatura);

            return Ok(new { solicitudId, folio, mensaje = "Solicitud registrada correctamente" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(string? estado = null, string? tipo = null, DateTime? fechaInicio = null, DateTime? fechaFin = null, string? areas = null)
        {
            int idusuario = int.Parse(Sesion.usuario());
            bool verTodo = await TieneVisibilidadGlobalAsync(idusuario);

            AD_TI_Solicitudes_Listado datos = new AD_TI_Solicitudes_Listado(CadenaConexion);
            var result = await datos.Listado(idusuario, verTodo, estado, tipo, fechaInicio, fechaFin, areas);
            return Ok(result);
        }

        // Identificador del usuario conectado (viene de Sesion.usuario(), el
        // mismo mecanismo que usa toda accion de este controller). No toca
        // ninguna tabla -- lo usa el frontend para decidir visualmente si el
        // usuario conectado es el solicitante de una solicitud (ej. mostrar
        // los botones de ResolverRevisionPrioridad). Es solo apoyo visual:
        // la autorizacion real sigue siendo la que valida cada SP.
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public ActionResult UsuarioActual()
        {
            int idusuario = int.Parse(Sesion.usuario());
            return Ok(new { usuarioId = idusuario });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener(long id)
        {
            int idusuario = int.Parse(Sesion.usuario());

            var solicitud = await new AD_TI_Solicitudes_Obtener(CadenaConexion).Obtener(id);
            if (solicitud == null)
                return NotFound(new { mensaje = "Solicitud no encontrada" });

            if (!await TieneAccesoAsync(solicitud, idusuario))
                return Forbid();

            return Ok(solicitud);
        }

        // Dueño/creador de la solicitud, o Jefatura de TI / Admin del modulo.
        // El SP ademas bloquea la edicion si el alcance ya fue aprobado.
        private async Task<bool> PuedeEditarSolicitudAsync(mdl_TI_Solicitud solicitud, int idusuario)
        {
            if (solicitud.SolicitanteUsuarioId == idusuario || solicitud.CreadaPorUsuarioId == idusuario)
                return true;

            return await PuedeCambiarEstadoAsync(idusuario);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Editar(mdl_TI_SolicitudEditar mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());

            var solicitud = await new AD_TI_Solicitudes_Obtener(CadenaConexion).Obtener(mdl.SolicitudId);
            if (solicitud == null)
                return NotFound(new { mensaje = "Solicitud no encontrada" });

            if (!await PuedeEditarSolicitudAsync(solicitud, idusuario))
                return Forbid();

            await new AD_TI_Solicitudes_Editar(CadenaConexion).Editar(mdl, idusuario);
            return Ok(new { mensaje = "Solicitud actualizada correctamente" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Historial(long id)
        {
            int idusuario = int.Parse(Sesion.usuario());

            var solicitud = await new AD_TI_Solicitudes_Obtener(CadenaConexion).Obtener(id);
            if (solicitud == null)
                return NotFound(new { mensaje = "Solicitud no encontrada" });

            if (!await TieneAccesoAsync(solicitud, idusuario))
                return Forbid();

            var result = await new AD_TI_Solicitudes_Historial(CadenaConexion).Historial(id);
            return Ok(result);
        }

        // Transiciones validas en esta fase (registro/revision) y motivo
        // obligatorio para RECHAZADA/CANCELADA se validan tambien aqui;
        // la validacion definitiva de que la transicion exista para el
        // estado actual la hace el SP (RAISERROR >= 50000 -> 400).
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CambiarEstado(mdl_TI_CambioEstado mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());

            if (!await PuedeCambiarEstadoAsync(idusuario))
                return Forbid();

            if ((mdl.EstadoNuevoCodigo == "RECHAZADA" || mdl.EstadoNuevoCodigo == "CANCELADA") && string.IsNullOrWhiteSpace(mdl.Motivo))
                return BadRequest(new { mensaje = "El motivo es obligatorio para este cambio de estado" });

            await new AD_TI_Solicitudes_CambiarEstado(CadenaConexion).CambiarEstado(mdl, idusuario);
            return Ok(new { mensaje = "Estado actualizado" });
        }

        // Abierto a cualquier usuario autenticado: el SP valida que
        // @usuarioId sea realmente el SolicitanteUsuarioId de la solicitud
        // (identidad real, no rol) antes de resolver la revision de
        // prioridad. A proposito NO se gatea por PuedeCambiarEstadoAsync
        // (eso excluiria siempre al solicitante) ni se expone a traves de
        // CambiarEstado -- ver la nota de seguridad en
        // 10_RevisionPrioridadFuncionalidad.sql.
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ResolverRevisionPrioridad(mdl_TI_RevisionPrioridadResolver mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());

            if (mdl.EstadoResolucion == "RECHAZADA" && string.IsNullOrWhiteSpace(mdl.Motivo))
                return BadRequest(new { mensaje = "El motivo es obligatorio para rechazar la revision" });

            await new AD_TI_Solicitudes_ResolverRevisionPrioridad(CadenaConexion).Resolver(mdl, idusuario);
            return Ok(new { mensaje = "Revision de prioridad resuelta" });
        }

        // Resumen de solo lectura para la pantalla de seguimiento: version de
        // alcance / estimacion / evaluacion / decision vigentes de la
        // solicitud, con el detalle de sus aprobaciones. Misma regla de
        // acceso que Obtener/Historial.
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ResumenSeguimiento(long id)
        {
            int idusuario = int.Parse(Sesion.usuario());

            var solicitud = await new AD_TI_Solicitudes_Obtener(CadenaConexion).Obtener(id);
            if (solicitud == null)
                return NotFound(new { mensaje = "Solicitud no encontrada" });

            if (!await TieneAccesoAsync(solicitud, idusuario))
                return Forbid();

            var resumen = await new AD_TI_Solicitudes_ResumenSeguimiento(CadenaConexion).Obtener(id);
            return Ok(resumen);
        }

        // Elimina la solicitud por completo (no es un soft-delete), sin
        // importar su estatus actual. Solo Jefatura de TI. El front exige
        // escribir el folio exacto como confirmacion antes de llamar aqui.
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Eliminar(mdl_TI_SolicitudEliminar mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            await new AD_TI_Solicitudes_Eliminar(CadenaConexion).Eliminar(mdl.SolicitudId, idusuario);
            return Ok(new { mensaje = "Solicitud eliminada correctamente" });
        }
    }
}
