using Dapper;
using HD.AccesoDatos;
using HD.Endpoints.Controllers;
using HD.Generales.Consultas;
using HD.Notifications.SeguimientoActividades;
using HD.Security;
using HD_GestionActividades.Consultas.SeguimientoAct;
using HD_GestionActividades.Modelos;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace HD.Endpoints.Controllers.GestionActividades
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeguimientoActController : MyBase
    {
        private readonly IConfiguration _configuracion;
        private readonly ISesion _session;

        // Rol con el que un usuario puede levantar un ticket a nombre de otra
        // sucursal/departamento. Debe coincidir con el código validado en el
        // frontend (SeguimientoActividadesNuevaScreen.js).
        private const string ROL_ADMIN_SOPORTE = "ADTI";

        // Una vez que el ticket llega a uno de estos estatus ya no se cambia
        // más manualmente desde aquí (el flujo de calificación/reactivación
        // del creador es aparte, en SeguimientoAct/Calificar y /Reactivar).
        private static readonly string[] ESTATUS_TERMINALES = { "F", "A", "R" };

        // Las opciones para cambiar el estatus dependen del tipo de sala del
        // ticket, no de un flujo fijo: las salas "normales" se trabajan
        // (Proceso/Finalizado) y las de "autorización" (ej. Confirmación de
        // depósitos) solo se aprueban o rechazan (Autorizado/Rechazado).
        // Debe coincidir con obtenerTransicionesDisponibles en el frontend
        // (Helpers/SeguimientoActEstatus.js) -- se valida aquí también
        // porque no hay que confiar en lo que mande el cliente.
        private static string[] CandidatosPorTipoSala(string? tipoSala)
        {
            return tipoSala == "A"
                ? new[] { "A", "R" }
                : new[] { "P", "F" };
        }

        public SeguimientoActController(IConfiguration configuracion, ISesion session)
        {
            _configuracion = configuracion;
            _session = session;
        }

        // Verifica contra la base (no contra lo que mande el cliente) si el
        // usuario actual tiene el rol de administrador de soporte. Se apoya
        // en AD_ValidateUser (mismos datos que arma el login), sin repetir
        // el flujo completo de autenticación. Ante cualquier falla al
        // consultarlo, se asume que NO es admin (fail-closed).
        private async Task<bool> EsAdminSoporteAsync(int idUsuario)
        {
            try
            {
                string cadenaConexionLogin = _configuracion["ConnectionStrings:Login"];
                var datosSesion = await new AD_ValidateUser(cadenaConexionLogin)
                    .UsuarioSesion(idUsuario.ToString());

                return datosSesion?.roles?.Any(r => r.idrol == ROL_ADMIN_SOPORTE) ?? false;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] mdl_SeguimientoAct seguimiento)
        {
            try
            {
                if (seguimiento == null)
                    return BadRequest(new { mensaje = "Datos inválidos" });

                int usuarioActual = int.Parse(_session.usuario());

                seguimiento.usuario = usuarioActual;

                // Blindaje: idSucursal/idDepartamento solo se persisten si el
                // usuario realmente tiene el rol ADTI verificado en el
                // servidor. Cualquier otro usuario que los mande en el body
                // los pierde aquí, sin importar lo que haya enviado el front.
                if (!await EsAdminSoporteAsync(usuarioActual))
                {
                    seguimiento.idSucursal = null;
                    seguimiento.idDepartamento = null;
                }

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_SeguimientoAct ad = new AD_SeguimientoAct(cadenaConexion);

                int idGenerado = await ad.GuardarAsync(seguimiento);

                var data = await ad.ObtenerAsync(idGenerado, usuarioActual);

                var modeloCorreo = new mdlSeguimiento_Email
                {
                    idSala = seguimiento.idSala,
                    idSolicitud = idGenerado,
                    folio = data.folio,
                    nombreSala = data.nombreSala,
                    actividad = data.nombreActividad,
                    comentarios = data.comentarios,
                    estatus = data.estatus,
                    usuario = data.usuarioNombre,
                    accionPor = data.usuarioNombre
                };

                // Ticket recién creado -- se avisa a los responsables de la
                // sala para que se enteren de que hay algo nuevo por atender.
                var destinatariosCreacion = CorreosSeguimientoAct.ObtenerCorreosResponsables(seguimiento.idSala, cadenaConexion);
                await NotificacionSeguimientoAct.Enviar(modeloCorreo, destinatariosCreacion);

                return Ok(new { mensaje = "Guardado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("Editar")]
        public async Task<IActionResult> Editar([FromBody] mdl_SeguimientoAct seguimiento)
        {
            try
            {
                if (seguimiento == null || seguimiento.idSolicitud == 0)
                    return BadRequest(new { mensaje = "Datos inválidos" });

                int usuarioActual = int.Parse(_session.usuario());
                seguimiento.usuario = usuarioActual;

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_SeguimientoAct ad = new AD_SeguimientoAct(cadenaConexion);

                await ad.EditarAsync(seguimiento);

                var data = await ad.ObtenerAsync(seguimiento.idSolicitud, usuarioActual);

                // Quien edita puede ser el responsable o el propio creador
                // (ver comentario de la ruta) -- se muestra a quién
                // corresponde en el correo, no siempre al creador.
                var contactoEditor = CorreosSeguimientoAct.ObtenerContactoEmpleado(usuarioActual, cadenaConexion);

                var modeloCorreo = new mdlSeguimiento_Email
                {
                    idSala = data.idSala,
                    idSolicitud = data.idSolicitud,
                    folio = data.folio,
                    nombreSala = data.nombreSala,
                    actividad = data.nombreActividad,
                    comentarios = data.comentarios,
                    estatus = data.estatus,
                    usuario = data.usuarioNombre,
                    accionPor = contactoEditor?.Nombre ?? data.usuarioNombre
                };

                var destinatariosEdicion = CorreosSeguimientoAct.ObtenerCorreosResponsables(data.idSala, cadenaConexion);
                await NotificacionSeguimientoAct.Enviar(modeloCorreo, destinatariosEdicion);

                return Ok(new { mensaje = "Seguimiento editado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        // Cambia el estatus del ticket -- separado de Editar() a propósito:
        // Editar() es para el contenido (sala/actividad/comentario) y lo
        // pueden usar el responsable o el creador; esto es exclusivamente
        // para el flujo de atención del ticket y solo lo puede hacer el
        // responsable de la sala, verificado aquí contra la base (no contra
        // lo que mande el cliente), igual que el blindaje de sucursal en
        // Guardar().
        [HttpPost("CambiarEstatus")]
        public async Task<IActionResult> CambiarEstatus([FromBody] mdl_SeguimientoAct model)
        {
            try
            {
                if (model == null || model.idSolicitud == 0 || string.IsNullOrEmpty(model.estatus))
                    return BadRequest(new { mensaje = "Datos inválidos" });

                int usuarioActual = int.Parse(_session.usuario());

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_SeguimientoAct ad = new AD_SeguimientoAct(cadenaConexion);

                var ticket = await ad.ObtenerAsync(model.idSolicitud, usuarioActual);

                if (ticket == null)
                    return NotFound(new { mensaje = "Ticket no encontrado" });

                if (ticket.esResponsable != 1)
                    return BadRequest(new { mensaje = "Solo el responsable de esta sala puede cambiar el estatus de este ticket" });

                if (ESTATUS_TERMINALES.Contains(ticket.estatus))
                    return BadRequest(new { mensaje = $"El ticket ya está en un estatus final (\"{ticket.estatus}\") y no se puede modificar" });

                var candidatos = CandidatosPorTipoSala(ticket.tipoSala).Where(c => c != ticket.estatus);

                if (!candidatos.Contains(model.estatus))
                    return BadRequest(new { mensaje = $"No se puede cambiar el estatus de \"{ticket.estatus}\" a \"{model.estatus}\"" });

                await ad.CambiarEstatusAsync(model.idSolicitud, model.estatus, usuarioActual);

                // El comentario del cambio de estatus es opcional -- si lo
                // mandan, se guarda igual que cualquier otro comentario del
                // historial (reutiliza SP_Cat_SeguimientoAct_AgregarComentario,
                // que ya existe, sin tocar SQL nuevo).
                if (!string.IsNullOrWhiteSpace(model.comentario))
                    await ad.AgregarComentarioAsync(model.idSolicitud, model.comentario, usuarioActual);

                var data = await ad.ObtenerAsync(model.idSolicitud, usuarioActual);

                // Quien cambia el estatus siempre es el responsable (ya se
                // validó arriba) -- el correo debe avisarle a quien LEVANTÓ
                // el ticket, no a los propios responsables.
                var contactoResponsable = CorreosSeguimientoAct.ObtenerContactoEmpleado(usuarioActual, cadenaConexion);
                var contactoCreador = CorreosSeguimientoAct.ObtenerContactoEmpleado(data.createUser, cadenaConexion);

                var modeloCorreo = new mdlSeguimiento_Email
                {
                    idSala = data.idSala,
                    idSolicitud = data.idSolicitud,
                    folio = data.folio,
                    nombreSala = data.nombreSala,
                    actividad = data.nombreActividad,
                    comentarios = model.comentario,
                    estatus = data.estatus,
                    usuario = data.usuarioNombre,
                    accionPor = contactoResponsable?.Nombre ?? data.usuarioNombre
                };

                var destinatariosEstatus = !string.IsNullOrWhiteSpace(contactoCreador?.Correo)
                    ? new List<string> { contactoCreador!.Correo! }
                    : new List<string>();

                await NotificacionSeguimientoAct.Enviar(modeloCorreo, destinatariosEstatus);

                return Ok(new { mensaje = "Estatus actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("Listado")]
        public async Task<IActionResult> Listado()
        {
            try
            {
                int usuarioActual = int.Parse(_session.usuario());

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_SeguimientoAct ad = new AD_SeguimientoAct(cadenaConexion);

                var resultado = await ad.ListadoAsync(usuarioActual);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("UsuarioActual")]
        public IActionResult UsuarioActual()
        {
            return Ok(new { idUsuario = int.Parse(_session.usuario()) });
        }

        [HttpGet("Obtener/{id}")]
        public async Task<IActionResult> Obtener(int id)
        {
            try
            {
                int usuarioActual = int.Parse(_session.usuario());

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_SeguimientoAct ad = new AD_SeguimientoAct(cadenaConexion);

                var resultado = await ad.ObtenerAsync(id, usuarioActual);

                if (resultado == null)
                    return NotFound(new { mensaje = "Seguimiento no encontrado" });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("Calificar")]
        public async Task<IActionResult> Calificar([FromBody] mdl_SeguimientoAct model)
        {
            try
            {
                int usuarioActual = int.Parse(_session.usuario());

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_SeguimientoAct ad = new AD_SeguimientoAct(cadenaConexion);

                if (model.calificacion == null || model.calificacion == 0)
                    return BadRequest(new { mensaje = "Calificación inválida" });

                await ad.CalificarAsync(
                    model.idSolicitud,
                    model.calificacion.Value,
                    model.comentario,
                    usuarioActual
                );

                return Ok(new { mensaje = "Calificado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("EliminarPorID")]
        public async Task<IActionResult> EliminarPorID([FromBody] int idSolicitud)
        {
            try
            {
                if (idSolicitud == 0)
                    return BadRequest(new { mensaje = "ID inválido" });

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];

                AD_SeguimientoAct ad = new AD_SeguimientoAct(cadenaConexion);

                await ad.EliminarPorIDAsync(idSolicitud, int.Parse(_session.usuario()));

                return Ok(new { mensaje = "Registro eliminado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("Evidencia/{id}")]
        public async Task<IActionResult> Evidencia(int id)
        {
            try
            {
                int usuarioActual = int.Parse(_session.usuario());

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_SeguimientoAct ad = new AD_SeguimientoAct(cadenaConexion);

                var resultado = await ad.ObtenerAsync(id, usuarioActual); 

                if (resultado == null)
                    return NotFound(new { mensaje = "No se encontró evidencia" });

                return Ok(new { evidencia = resultado.evidencia });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }


        [HttpGet("Historial/{id}")]
        public async Task<IActionResult> Historial(int id)
        {
            try
            {
                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];

                AD_SeguimientoAct ad = new AD_SeguimientoAct(cadenaConexion);

                var resultado = await ad.HistorialAsync(id);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("Notificaciones")]
        public async Task<IActionResult> Notificaciones()
        {
            int usuarioActual = int.Parse(_session.usuario());

            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_SeguimientoAct ad = new AD_SeguimientoAct(cadenaConexion);

            int total = await ad.ConteoNoRevisadosAsync(usuarioActual);

            return Ok(new { total });
        }

        [HttpPost("MarcarRevisado")]
        public async Task<IActionResult> MarcarRevisado()
        {
            int usuarioActual = int.Parse(_session.usuario());

            string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
            AD_SeguimientoAct ad = new AD_SeguimientoAct(cadenaConexion);

            await ad.MarcarRevisadoAsync(usuarioActual);

            return Ok(new { mensaje = "Notificaciones marcadas como revisadas" });
        }


        [HttpPost("AgregarComentario")]
        public async Task<IActionResult> AgregarComentario([FromBody] mdl_SeguimientoAct model)
        {
            try
            {
                if (model == null || model.idSolicitud == 0)
                    return BadRequest(new { mensaje = "Datos inválidos" });

                int usuarioActual = int.Parse(_session.usuario());

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];

                AD_SeguimientoAct ad = new AD_SeguimientoAct(cadenaConexion);

                
                await ad.AgregarComentarioAsync(model.idSolicitud, model.comentario, usuarioActual);

                var data = await ad.ObtenerAsync(model.idSolicitud, usuarioActual);

                // "Y viceversa": si comenta el creador se avisa a los
                // responsables de la sala; si comenta cualquier otra
                // persona (típicamente el responsable atendiendo el
                // ticket) se avisa a quien lo levantó.
                bool actorEsCreador = data.createUser == usuarioActual;

                List<string> destinatariosComentario;
                string? nombreActor;

                if (actorEsCreador)
                {
                    destinatariosComentario = CorreosSeguimientoAct.ObtenerCorreosResponsables(data.idSala, cadenaConexion);
                    nombreActor = data.usuarioNombre;
                }
                else
                {
                    var contactoCreador = CorreosSeguimientoAct.ObtenerContactoEmpleado(data.createUser, cadenaConexion);
                    var contactoActor = CorreosSeguimientoAct.ObtenerContactoEmpleado(usuarioActual, cadenaConexion);

                    destinatariosComentario = !string.IsNullOrWhiteSpace(contactoCreador?.Correo)
                        ? new List<string> { contactoCreador!.Correo! }
                        : new List<string>();
                    nombreActor = contactoActor?.Nombre;
                }

                var modeloCorreo = new mdlSeguimiento_Email
                {
                    idSala = data.idSala,
                    idSolicitud = data.idSolicitud,
                    folio = data.folio,
                    nombreSala = data.nombreSala,
                    actividad = data.nombreActividad,
                    comentarios = model.comentario,
                    estatus = "M",
                    usuario = data.usuarioNombre,
                    accionPor = nombreActor
                };

                await NotificacionSeguimientoActComentario.Enviar(modeloCorreo, destinatariosComentario, paraCreador: !actorEsCreador);

                return Ok(new { mensaje = "Comentario agregado correctamente" });
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpPost("Reactivar")]
        public async Task<IActionResult> Reactivar([FromBody] mdl_SeguimientoAct model)
        {
            try
            {
                int usuarioActual = int.Parse(_session.usuario());

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_SeguimientoAct ad = new AD_SeguimientoAct(cadenaConexion);

                await ad.ReactivarAsync(model.idSolicitud, model.comentario, usuarioActual);

                return Ok(new { mensaje = "Reactivado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}