using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Consultas.Credito_Condicionado;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using HD.Notifications.Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.Credito_Condicionado
{
    public class SCFechaCompromisoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SCFechaCompromisoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Enviar(mdl_fecha_compromiso mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Credito_Condicionado_Fecha_Compromiso_Guardar datos = new AD_Credito_Condicionado_Fecha_Compromiso_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            //if (result.mdldatos is null)
            //{
            //    return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            //}
            //await NotificacionComentarios.EnviarOperacionCondicionada(result);
            return Ok(result.datos_fecha);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Obtener(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Credito_Condicionado_Fecha_Compromiso_Obtener datos = new AD_Credito_Condicionado_Fecha_Compromiso_Obtener(CadenaConexion);
            var result = await datos.Get(folio);
            return Ok(result);

        }
    }
}
