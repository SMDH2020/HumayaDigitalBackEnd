using HD.Security;
using HD_GestionProyectosTI.Consultas;
using HD_GestionProyectosTI.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.GestionProyectosTI
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpPost("Crear")]
        public async Task<ActionResult> Crear(mdl_SolicitudCrear mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            mdl.idusuario_solicitante = idusuario;

            // Las 5 preguntas solo aplican (y son obligatorias) para "Acceso a
            // información" -- para Incidencia/Nuevo desarrollo/Mejora se
            // ignoran aunque vengan en el body, para no arrastrar texto que no
            // corresponde a ese tipo de solicitud.
            if (mdl.tipo == "Acceso a información")
            {
                bool faltaAlguna =
                    string.IsNullOrWhiteSpace(mdl.pregunta_informacion) ||
                    string.IsNullOrWhiteSpace(mdl.pregunta_objetivo_negocio) ||
                    string.IsNullOrWhiteSpace(mdl.pregunta_decisiones) ||
                    string.IsNullOrWhiteSpace(mdl.pregunta_frecuencia) ||
                    string.IsNullOrWhiteSpace(mdl.pregunta_uso_compartido);

                if (faltaAlguna)
                    return BadRequest(new { mensaje = "Las 5 preguntas son obligatorias para una solicitud de acceso a información" });
            }
            else
            {
                mdl.pregunta_informacion = null;
                mdl.pregunta_objetivo_negocio = null;
                mdl.pregunta_decisiones = null;
                mdl.pregunta_frecuencia = null;
                mdl.pregunta_uso_compartido = null;
            }

            // El impacto de negocio solo aplica a Nuevo desarrollo / Mejora.
            if (mdl.tipo != "Nuevo desarrollo" && mdl.tipo != "Mejora")
            {
                mdl.impacto_control_interno = false;
                mdl.impacto_normativo = false;
                mdl.impacto_financiero = false;
                mdl.impacto_comentario = null;
            }

            AD_Solicitudes datos = new AD_Solicitudes(CadenaConexion);
            int idsolicitud = await datos.Crear(mdl);
            return Ok(new { idsolicitud, mensaje = "Solicitud creada correctamente" });
        }

        // El listado se filtra solo con el rol del que consulta: un Usuario
        // solo ve las suyas, un Developer las suyas + donde tiene actividades
        // asignadas, un Admin ve todo. Ver sp_Solicitudes_Listado.
        [HttpGet("Listado")]
        public async Task<ActionResult> Listado(string? estado = null, string? tipo = null)
        {
            int idusuario = int.Parse(Sesion.usuario());
            string rol = await new AD_RolTI(CadenaConexion).Obtener(idusuario);

            AD_Solicitudes datos = new AD_Solicitudes(CadenaConexion);
            var result = await datos.Listado(idusuario, rol, estado, tipo);
            return Ok(result);
        }

        [HttpGet("Obtener/{id}")]
        public async Task<ActionResult> Obtener(int id)
        {
            int idusuario = int.Parse(Sesion.usuario());
            string rol = await new AD_RolTI(CadenaConexion).Obtener(idusuario);

            AD_Solicitudes datos = new AD_Solicitudes(CadenaConexion);
            var solicitud = await datos.Obtener(id);

            if (solicitud == null)
                return NotFound(new { mensaje = "Solicitud no encontrada" });

            // Un Usuario solo puede abrir sus propias solicitudes.
            if (rol == "Usuario" && solicitud.idusuario_solicitante != idusuario)
                return Forbid();

            return Ok(solicitud);
        }

        // Solo Admin: revisar -> aceptar/rechazar, pasar a definicion, pasar
        // a pendiente de aprobacion, cancelar. motivo obligatorio para
        // 'Rechazada' y 'Cancelada'.
        [HttpPost("CambiarEstado")]
        public async Task<ActionResult> CambiarEstado(mdl_CambioEstado mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            string rol = await new AD_RolTI(CadenaConexion).Obtener(idusuario);
            if (rol != "Admin")
                return Forbid();

            if ((mdl.estado_nuevo == "Rechazada" || mdl.estado_nuevo == "Cancelada") && string.IsNullOrWhiteSpace(mdl.motivo))
                return BadRequest(new { mensaje = "El motivo es obligatorio para este cambio de estado" });

            await new AD_Solicitudes(CadenaConexion).CambiarEstado(mdl, idusuario);
            return Ok(new { mensaje = "Estado actualizado" });
        }

        // Solo el solicitante dueño de la solicitud, y solo mientras está en
        // 'Pendiente de aprobación del usuario'. Si aprueba, vuelve a
        // 'Aceptada' (alcance firmado, listo para que TI lo priorice). Si
        // rechaza, vuelve a 'En definición' y motivo es obligatorio; el SP
        // incrementa el contador de rondas de rechazo automáticamente.
        [HttpPost("ResponderAprobacionAlcance")]
        public async Task<ActionResult> ResponderAprobacionAlcance(int idsolicitud, bool aprueba, string? motivo)
        {
            int idusuario = int.Parse(Sesion.usuario());

            var solicitud = await new AD_Solicitudes(CadenaConexion).Obtener(idsolicitud);
            if (solicitud == null)
                return NotFound(new { mensaje = "Solicitud no encontrada" });
            if (solicitud.idusuario_solicitante != idusuario)
                return Forbid();
            if (solicitud.estado != "Pendiente de aprobación del usuario")
                return BadRequest(new { mensaje = "La solicitud no está esperando aprobación de alcance" });

            if (!aprueba && string.IsNullOrWhiteSpace(motivo))
                return BadRequest(new { mensaje = "El motivo es obligatorio para rechazar el alcance" });

            var cambio = new mdl_CambioEstado
            {
                idsolicitud = idsolicitud,
                estado_nuevo = aprueba ? "Aceptada" : "En definición",
                motivo = motivo
            };
            await new AD_Solicitudes(CadenaConexion).CambiarEstado(cambio, idusuario);
            return Ok(new { mensaje = aprueba ? "Alcance aprobado" : "Alcance rechazado, regresa a definición" });
        }

        // Solo Admin. Si la solicitud ya había sido priorizada antes, el SP
        // la deja en 'Repriorizada' y aquí exigimos motivo.
        [HttpPost("Priorizar")]
        public async Task<ActionResult> Priorizar(mdl_Priorizar mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            string rol = await new AD_RolTI(CadenaConexion).Obtener(idusuario);
            if (rol != "Admin")
                return Forbid();

            var actual = await new AD_Solicitudes(CadenaConexion).Obtener(mdl.idsolicitud);
            bool yaPriorizada = actual != null && new[] { "Priorizada", "Repriorizada", "Asignada", "En progreso", "En pruebas" }.Contains(actual.estado);
            if (yaPriorizada && string.IsNullOrWhiteSpace(mdl.motivo))
                return BadRequest(new { mensaje = "El motivo es obligatorio para repriorizar" });

            await new AD_Solicitudes(CadenaConexion).Priorizar(mdl, idusuario);
            return Ok(new { mensaje = "Prioridad actualizada" });
        }

        // Solo Admin. Motivo siempre obligatorio.
        [HttpPost("CambiarFechaComprometida")]
        public async Task<ActionResult> CambiarFechaComprometida(mdl_CambioFecha mdl)
        {
            int idusuario = int.Parse(Sesion.usuario());
            string rol = await new AD_RolTI(CadenaConexion).Obtener(idusuario);
            if (rol != "Admin")
                return Forbid();

            if (string.IsNullOrWhiteSpace(mdl.motivo))
                return BadRequest(new { mensaje = "El motivo es obligatorio" });

            await new AD_Solicitudes(CadenaConexion).CambiarFechaComprometida(mdl, idusuario);
            return Ok(new { mensaje = "Fecha comprometida actualizada" });
        }
    }
}
