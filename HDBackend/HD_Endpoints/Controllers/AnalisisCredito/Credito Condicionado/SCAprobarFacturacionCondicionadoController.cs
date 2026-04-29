using HD.Clientes.Consultas.Credito_Condicionado;
using HD.Clientes.Modelos.SC_Analisis.Credito_Condicionados;
using HD.Notifications.Analisis;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.Credito_Condicionado
{
    public class SCAprobarFacturacionCondicionadoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SCAprobarFacturacionCondicionadoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Enviar(mdl_fecha_compromiso mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Credito_Condicionado_Aprobar_Facturacion datos = new AD_Credito_Condicionado_Aprobar_Facturacion(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            //if (result.mdldatos is null)
            //{
            //    return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            //}
            //await NotificacionComentarios.EnviarOperacionCondicionada(result);
            return Ok(result.datos_fecha);

        }
    }
}
