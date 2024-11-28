using HD.Notifications.Analisis;
using HD.Notifications.ClientesJuridico;
using HD.Security;
using HD_Cobranza.Capturas.ConvenioPago;
using HD_Cobranza.Capturas.Juridico;
using HD_Cobranza.Consultas.Juridico;
using HD_Cobranza.GestionCobranza.Capturas;
using HD_Cobranza.GestionCobranza.Modelos;
using HD_Cobranza.Modelos.Juridico;
using HD_Reporteria.GestionCobranza;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Cobranza
{
    public class ClientesEnviarJuridicoController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClientesEnviarJuridicoController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        //Cobranza
        [HttpPost]
        public async Task<ActionResult> Guardar(mdl_Clientes_Juridico_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Juridicos_Guardar datos = new AD_Clientes_Juridicos_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            if (result is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            await NotificacionGestionClienteJuridico.Enviar(result);
            return Ok(result);          
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> Listado(string adr, string sucursal, string estatus)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Juridicos_Listado datos = new AD_Clientes_Juridicos_Listado(CadenaConexion);
            var result = await datos.Listado(adr, sucursal, estatus);
            return Ok(result);
        }

        //Credito

        [HttpPost]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GuardarCredito(mdl_Clientes_Juridico_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Juridico_Credito_Guardar datos = new AD_Clientes_Juridico_Credito_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            if (result is null)
            {
                return BadRequest(new { mensaje = "Error al enviar correo, no se encontro información" });
            }
            await NotificacionGestionClienteJuridico.Enviar(result);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> ListadoEnviados(string adr, string sucursal, string estatus)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Enviados_Juridico_Listado datos = new AD_Clientes_Enviados_Juridico_Listado(CadenaConexion);
            var result = await datos.Listado(adr, sucursal, estatus);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]

        public async Task<ActionResult> ObtenerComentarios(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Clientes_Enviados_Juridico_Obtener_Comentarios datos = new AD_Clientes_Enviados_Juridico_Obtener_Comentarios(CadenaConexion);
            var result = await datos.obtener(idcliente);
            return Ok(result);
        }
    }
}
