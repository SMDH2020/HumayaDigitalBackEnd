using HD.Notifications.GestionProyectosTI;
using HD.Notifications.Modelos;
using HD.Security;
using HD_GestionProyectosTI.Consultas.Priorizacion;
using HD_GestionProyectosTI.Consultas.Roles;
using HD_GestionProyectosTI.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionProyectosTI
{
    public class PriorizacionTIController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public PriorizacionTIController(IConfiguration configuracion, ISesion sesion)
        {
            Configuracion = configuracion;
            Sesion = sesion;
        }

        private string CadenaConexion => Configuracion["ConnectionStrings:GestionProyectosTI"]!;
        private mdl_Correo_M365 ConfigCorreo => Configuracion.GetSection("CorreoM365").Get<mdl_Correo_M365>()!;

        private async Task<bool> EsJefaturaTIAsync(int idusuario)
        {
            var roles = await new AD_TI_Roles_Mio(CadenaConexion).Mio(idusuario);
            return roles.Any(r => r.Codigo == "JEFATURA_TI");
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CrearEvaluacion(mdl_TI_EvaluacionCrear mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            mdl.EvaluadaPorUsuarioId = idusuario;

            var (evaluacionPrioridadId, numeroEvaluacion) = await new AD_TI_EvaluacionesPrioridad_Crear(CadenaConexion).Crear(mdl);
            return Ok(new { evaluacionPrioridadId, numeroEvaluacion, mensaje = "Evaluacion de prioridad registrada correctamente" });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> RegistrarDetalle(mdl_TI_EvaluacionRegistrarDetalle mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            await new AD_TI_EvaluacionesPrioridad_RegistrarDetalle(CadenaConexion).RegistrarDetalle(mdl, idusuario);
            return Ok(new { mensaje = "Detalle actualizado" });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Completar(long id)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            decimal puntajeTotal = await new AD_TI_EvaluacionesPrioridad_Completar(CadenaConexion).Completar(id, idusuario);
            return Ok(new { puntajeTotal, mensaje = "Evaluacion completada" });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> RegistrarDecision(mdl_TI_DecisionCrear mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            var (decisionPrioridadId, numeroDecision) = await new AD_TI_DecisionesPrioridad_Crear(CadenaConexion).Crear(mdl, idusuario);
            return Ok(new { decisionPrioridadId, numeroDecision, mensaje = "Decision de prioridad registrada correctamente" });
        }

        // Abierto a cualquier usuario autenticado: el SP valida que el
        // TipoAprobador corresponda al rol real del usuario (JEFATURA_TI o
        // GERENCIA_PLANEACION).
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AutorizarDecision(mdl_TI_AprobacionPrioridad mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());

            if (mdl.EstadoDecision == "RECHAZADA" && string.IsNullOrWhiteSpace(mdl.Comentario))
                return BadRequest(new { mensaje = "El comentario es obligatorio para rechazar" });

            await new AD_TI_AprobacionesPrioridad_Registrar(CadenaConexion).Registrar(mdl, idusuario);
            return Ok(new { mensaje = "Aprobacion registrada" });
        }

        // ------------------------------------------------------------------
        // BLOQUE 2: programacion mensual por funcionalidad. La programacion
        // inicial no requiere aprobacion individual de Planeacion (el SP
        // valida que exista una decision CONFIRMADA/PRIORIZAR); los cambios
        // posteriores si la requieren (ver ResolverCambioProgramacion).
        // ------------------------------------------------------------------

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ProgramarInicial(mdl_TI_FuncionalidadProgramarInicial mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            await new AD_TI_Funcionalidades_ProgramarInicial(CadenaConexion).ProgramarInicial(mdl, idusuario);

            // Aviso al solicitante de que su funcionalidad ya tiene mes/año
            // asignado. El contexto (solicitud/folio/solicitante) se
            // resuelve aparte porque ProgramarInicial no lo regresa.
            var contexto = await new AD_TI_Funcionalidades_ContextoCorreo(CadenaConexion).Obtener(mdl.FuncionalidadId);
            if (contexto != null && !string.IsNullOrWhiteSpace(contexto.SolicitanteCorreo))
            {
                await NotificacionTI.EnviarFuncionalidadPriorizada(
                    ConfigCorreo, contexto.Folio, contexto.Titulo, contexto.Nombre,
                    contexto.MesProgramado, contexto.AnioProgramado,
                    new List<string> { contexto.SolicitanteCorreo! });
            }

            return Ok(new { mensaje = "Funcionalidad programada correctamente" });
        }

        // Accion directa de Jefatura de TI (sin aprobacion de Planeacion):
        // regresa una funcionalidad ya programada al backlog.
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> RegresarABacklog(mdl_TI_FuncionalidadRegresarBacklog mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            await new AD_TI_Funcionalidades_RegresarABacklog(CadenaConexion).RegresarABacklog(mdl, idusuario);
            return Ok(new { mensaje = "Funcionalidad regresada al backlog" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ProgramacionVigente(long idsolicitud)
        {
            var result = await new AD_TI_Funcionalidades_ProgramacionVigente_ListadoPorSolicitud(CadenaConexion).Listado(idsolicitud);
            return Ok(result);
        }

        // Jefatura de TI envia una funcionalidad ya programada a revision
        // del solicitante. Mientras quede PENDIENTE, el propio SP bloquea
        // Programar/Regresar a Backlog para esa funcionalidad.
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EnviarARevision(mdl_TI_FuncionalidadEnviarRevision mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            var cambioProgramacionId = await new AD_TI_Funcionalidades_EnviarARevision(CadenaConexion).EnviarARevision(mdl, idusuario);

            // Aviso al solicitante de que debe aprobar o rechazar esta
            // funcionalidad.
            var contexto = await new AD_TI_Funcionalidades_ContextoCorreo(CadenaConexion).Obtener(mdl.FuncionalidadId);
            if (contexto != null && !string.IsNullOrWhiteSpace(contexto.SolicitanteCorreo))
            {
                await NotificacionTI.EnviarFuncionalidadEnRevision(
                    ConfigCorreo, contexto.Folio, contexto.Titulo, contexto.Nombre,
                    contexto.MesProgramado, contexto.AnioProgramado,
                    new List<string> { contexto.SolicitanteCorreo! });
            }

            return Ok(new { cambioProgramacionId, mensaje = "Funcionalidad enviada a revision" });
        }

        // Abierto a cualquier usuario autenticado: el SP valida que
        // @usuarioId sea realmente el SolicitanteUsuarioId de la solicitud
        // duena de la funcionalidad (identidad real, no rol). A proposito
        // NO se gatea por EsJefaturaTIAsync -- eso excluiria siempre al
        // solicitante.
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ResolverRevision(mdl_TI_FuncionalidadResolverRevision mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());

            if (mdl.EstadoResolucion == "RECHAZADO" && string.IsNullOrWhiteSpace(mdl.ComentarioResolucion))
                return BadRequest(new { mensaje = "El comentario es obligatorio para rechazar" });

            await new AD_TI_Funcionalidades_ResolverRevision(CadenaConexion).ResolverRevision(mdl, idusuario);

            // Aviso a Jefatura de TI de que el solicitante ya aprobo o
            // rechazo la revision. El contexto se resuelve a partir del
            // mismo CambioProgramacionId que ya se acaba de resolver.
            var contexto = await new AD_TI_CambiosProgramacion_ContextoCorreo(CadenaConexion).Obtener(mdl.CambioProgramacionId);
            if (contexto != null)
            {
                var jefaturaTI = await new AD_TI_Roles_UsuariosPorRol(CadenaConexion).Listado("JEFATURA_TI");
                var destinatarios = jefaturaTI.Select(u => u.Correo).Where(c => !string.IsNullOrWhiteSpace(c)).Select(c => c!).ToList();
                await NotificacionTI.EnviarRevisionResuelta(
                    ConfigCorreo, contexto.Folio, contexto.Titulo, contexto.Nombre, contexto.SolicitanteNombre ?? "",
                    mdl.EstadoResolucion == "APROBADO", mdl.ComentarioResolucion, destinatarios);
            }

            return Ok(new { mensaje = "Revision resuelta" });
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CrearCambioProgramacion(mdl_TI_CambioProgramacionCrear mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            var cambioProgramacionId = await new AD_TI_CambiosProgramacion_Crear(CadenaConexion).Crear(mdl, idusuario);
            return Ok(new { cambioProgramacionId, mensaje = "Cambio de programacion registrado, pendiente de aprobacion" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> CambiosProgramacion(long idsolicitud)
        {
            var result = await new AD_TI_CambiosProgramacion_ListadoPorSolicitud(CadenaConexion).Listado(idsolicitud);
            return Ok(result);
        }

        // Abierto a cualquier usuario autenticado: el SP valida que el
        // usuario tenga realmente el rol GERENCIA_PLANEACION.
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ResolverCambioProgramacion(mdl_TI_CambioProgramacionResolver mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());

            if (mdl.EstadoResolucion == "RECHAZADO" && string.IsNullOrWhiteSpace(mdl.ComentarioResolucion))
                return BadRequest(new { mensaje = "El comentario es obligatorio para rechazar" });

            await new AD_TI_CambiosProgramacion_Resolver(CadenaConexion).Resolver(mdl, idusuario);
            return Ok(new { mensaje = "Cambio de programacion resuelto" });
        }
    }
}
