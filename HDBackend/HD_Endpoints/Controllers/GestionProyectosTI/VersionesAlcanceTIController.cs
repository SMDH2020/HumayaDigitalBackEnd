using HD.Notifications.GestionProyectosTI;
using HD.Notifications.Modelos;
using HD.Security;
using HD_GestionProyectosTI.Consultas.Roles;
using HD_GestionProyectosTI.Consultas.Solicitudes;
using HD_GestionProyectosTI.Consultas.VersionesAlcance;
using HD_GestionProyectosTI.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionProyectosTI
{
    public class VersionesAlcanceTIController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;

        public VersionesAlcanceTIController(IConfiguration configuracion, ISesion sesion)
        {
            Configuracion = configuracion;
            Sesion = sesion;
        }

        private string CadenaConexion => Configuracion["ConnectionStrings:GestionProyectosTI"]!;
        private mdl_Correo_M365 ConfigCorreo => Configuracion.GetSection("CorreoM365").Get<mdl_Correo_M365>()!;

        // Crear/enviar una version de alcance es trabajo de Jefatura (la
        // matriz de permisos acordada no le da esta accion al solicitante
        // ni a ningun otro rol). La autorizacion fina de cada lado de la
        // aprobacion dual la hace el propio SP (TipoAprobador real).
        private async Task<bool> EsJefaturaTIAsync(int idusuario)
        {
            var roles = await new AD_TI_Roles_Mio(CadenaConexion).Mio(idusuario);
            return roles.Any(r => r.Codigo == "JEFATURA_TI");
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Crear(mdl_TI_VersionAlcanceCrear mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            mdl.UsuarioId = idusuario;

            var (versionAlcanceId, numeroVersion) = await new AD_TI_VersionesAlcance_Crear(CadenaConexion).Crear(mdl);
            return Ok(new { versionAlcanceId, numeroVersion, mensaje = "Version de alcance registrada correctamente" });
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener(long id)
        {
            var version = await new AD_TI_VersionesAlcance_Obtener(CadenaConexion).Obtener(id);
            if (version == null)
                return NotFound(new { mensaje = "Version de alcance no encontrada" });

            return Ok(version);
        }

        // Detalle de solo lectura de una funcionalidad (usado por el modal
        // "ver detalle" de Backlog Mensual, que no trae estos campos en su
        // listado). Abierto a cualquier usuario autenticado, igual que
        // Obtener(id) de arriba.
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerFuncionalidad(long id)
        {
            var funcionalidad = await new AD_TI_Funcionalidades_Obtener(CadenaConexion).Obtener(id);
            if (funcionalidad == null)
                return NotFound(new { mensaje = "Funcionalidad no encontrada" });

            return Ok(funcionalidad);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(long idsolicitud)
        {
            var result = await new AD_TI_VersionesAlcance_ListadoPorSolicitud(CadenaConexion).Listado(idsolicitud);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EnviarAprobacion(long id)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            await new AD_TI_VersionesAlcance_EnviarAprobacion(CadenaConexion).EnviarAprobacion(id, idusuario);
            return Ok(new { mensaje = "Version enviada a aprobacion" });
        }

        // Agrega una funcionalidad a una version ya creada. El SP rechaza la
        // operacion si la version ya tiene una aprobacion registrada (de
        // SOLICITANTE o de JEFATURA_TI) -- el bloqueo no depende solo de
        // ocultar el boton en pantalla.
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> AgregarFuncionalidad(mdl_TI_FuncionalidadAgregar mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            var funcionalidadId = await new AD_TI_Funcionalidades_Agregar(CadenaConexion).Agregar(mdl, idusuario);
            return Ok(new { funcionalidadId, mensaje = "Funcionalidad agregada correctamente" });
        }

        // Edita una funcionalidad ya registrada (codigo, nombre, descripcion,
        // reglas de negocio, criterios de aceptacion, complejidad, dias
        // estimados). El SP rechaza la operacion si la version ya tiene una
        // aprobacion registrada -- el bloqueo no depende solo del formulario.
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> EditarFuncionalidad(mdl_TI_FuncionalidadEditar mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            await new AD_TI_Funcionalidades_Editar(CadenaConexion).Editar(mdl, idusuario);
            return Ok(new { mensaje = "Funcionalidad actualizada correctamente" });
        }

        // Unica via para editar DiasEstimados despues de creada la version;
        // el SP rechaza si la version ya no esta en BORRADOR.
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ActualizarDiasFuncionalidad(mdl_TI_FuncionalidadActualizarDias mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            if (!await EsJefaturaTIAsync(idusuario))
                return Forbid();

            await new AD_TI_Funcionalidades_ActualizarDiasEstimados(CadenaConexion).ActualizarDiasEstimados(mdl, idusuario);
            return Ok(new { mensaje = "Dias estimados actualizados" });
        }

        // Abierto a cualquier usuario autenticado: el SP valida que el
        // TipoAprobador que se manda corresponda realmente a la relacion
        // del usuario con esta version (solicitante dueno / rol JEFATURA_TI).
        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Autorizar(mdl_TI_AprobacionAlcance mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());

            if (mdl.EstadoDecision == "RECHAZADA" && string.IsNullOrWhiteSpace(mdl.Comentario))
                return BadRequest(new { mensaje = "El comentario es obligatorio para rechazar" });

            await new AD_TI_AprobacionesAlcance_Registrar(CadenaConexion).Registrar(mdl, idusuario);

            // Si esta aprobacion fue la que completo el lado que faltaba,
            // la version ya quedo en APROBADA -- se avisa al solicitante y
            // a Jefatura de TI. Si todavia falta el otro lado, no se manda
            // nada (el SP solo lanza error si se intenta resolver dos veces
            // el mismo lado, asi que este re-fetch siempre refleja el
            // resultado real de esta llamada).
            var versionActualizada = await new AD_TI_VersionesAlcance_Obtener(CadenaConexion).Obtener(mdl.VersionAlcanceId);
            if (versionActualizada?.EstadoVersion == "APROBADA")
            {
                var solicitud = await new AD_TI_Solicitudes_Obtener(CadenaConexion).Obtener(versionActualizada.SolicitudId);
                if (solicitud != null)
                {
                    var jefaturaTI = await new AD_TI_Roles_UsuariosPorRol(CadenaConexion).Listado("JEFATURA_TI");
                    var destinatarios = jefaturaTI.Select(u => u.Correo).Where(c => !string.IsNullOrWhiteSpace(c)).Select(c => c!).ToList();
                    if (!string.IsNullOrWhiteSpace(solicitud.SolicitanteCorreo))
                        destinatarios.Add(solicitud.SolicitanteCorreo!);

                    await NotificacionTI.EnviarAlcanceAprobado(ConfigCorreo, solicitud.Folio, solicitud.Titulo, destinatarios);
                }
            }

            return Ok(new { mensaje = "Aprobacion registrada" });
        }
    }
}
