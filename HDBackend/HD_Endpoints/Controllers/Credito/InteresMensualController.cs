using HD.Clientes.Consultas.ClientesCultivo;
using HD.Clientes.Consultas.InteresMensual;
using HD.Clientes.Consultas.SolicitudCreditoDocumento;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;


namespace HD.Endpoints.Controllers.Credito
{
    public class InteresMensualController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public InteresMensualController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlInteres_Mensual mdl)
        {

            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_InteresMensual_Guardar datos = new AD_InteresMensual_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito", listado = result });

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarID(int idinteres)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_InteresMensual_ObtenerporID datos = new AD_InteresMensual_ObtenerporID(CadenaConexion);
            var result = await datos.BuscarID(idinteres);
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarEyP(int ejercicio, int periodo)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_InteresMensual_ObtenerEyP datos = new AD_InteresMensual_ObtenerEyP(CadenaConexion);
            var result = await datos.BuscarEyP(ejercicio, periodo);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int ejercicio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_InteresMensual_Listado datos = new AD_InteresMensual_Listado(CadenaConexion);
            var result = await datos.Listado(ejercicio);
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> ObtenerDocumento(int idinteres)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADInteres_Mensual_ObtenerDocumento datos = new ADInteres_Mensual_ObtenerDocumento(CadenaConexion);
            var result = await datos.Obtener(idinteres);
            if (result is null)
                return BadRequest(new { mensaje = "Documento no encontrado. Favor de comunicarse con el administrador del sistema" });
            return Ok(result);

        }
    }
}
