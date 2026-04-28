using Dapper;
using HD.AccesoDatos;
using HD.Endpoints.Controllers;
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

        public SeguimientoActController(IConfiguration configuracion, ISesion session)
        {
            _configuracion = configuracion;
            _session = session;
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

                string cadenaConexion = _configuracion["ConnectionStrings:Servicio"];
                AD_SeguimientoAct ad = new AD_SeguimientoAct(cadenaConexion);

                int idGenerado = await ad.GuardarAsync(seguimiento);

                var data = await ad.ObtenerAsync(idGenerado, usuarioActual);

                var modeloCorreo = new mdlSeguimiento_Email
                {
                    idSala = seguimiento.idSala, 
                    actividad = data.nombreActividad,
                    comentarios = data.comentarios,
                    estatus = data.estatus,
                    usuario = data.usuarioNombre
                };

                Console.WriteLine("ANTES DE ENVIAR CORREO");

                await NotificacionSeguimientoAct.Enviar(modeloCorreo, cadenaConexion);

                Console.WriteLine("DESPUES DE ENVIAR CORREO");

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

                var modeloCorreo = new mdlSeguimiento_Email
                {
                    idSala = data.idSala,
                    actividad = data.nombreActividad,
                    comentarios = data.comentarios,
                    estatus = data.estatus,
                    usuario = data.usuarioNombre
                };

                Console.WriteLine("ANTES DE ENVIAR CORREO EDITAR");

                
                await NotificacionSeguimientoAct.Enviar(modeloCorreo, cadenaConexion);

                Console.WriteLine("DESPUES DE ENVIAR CORREO EDITAR");

                return Ok(new { mensaje = "Seguimiento editado correctamente" });
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

                var modeloCorreo = new mdlSeguimiento_Email
                {
                    idSala = data.idSala,
                    actividad = data.nombreActividad,
                    comentarios = model.comentario,
                    estatus = "M", 
                    usuario = data.usuarioNombre
                };

                Console.WriteLine("ENVIANDO CORREO DE COMENTARIO");

                await NotificacionSeguimientoActComentario.Enviar(modeloCorreo, cadenaConexion);

                Console.WriteLine("CORREO ENVIADO");

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