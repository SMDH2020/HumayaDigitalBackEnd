using HD.Clientes.Consultas.ClientesDocumentacion;
using HD.Clientes.Modelos;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class ClienteDocumentacionController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ClienteDocumentacionController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Post(mdlClientesDocumentacion_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ClientesDocumentacion_Guardar datos = new AD_ClientesDocumentacion_Guardar(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result =await datos.Guardar(mdl);
            return Ok(new { mensaje = "datos cargados con exito", listado = result });

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(int idcliente)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ClientesDocumentacion_Listado datos = new AD_ClientesDocumentacion_Listado(CadenaConexion);
            var result = await datos.Listado(idcliente);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> BuscarID(string idclientedocumento)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_ClientesDocumentacion_ID datos = new AD_ClientesDocumentacion_ID(CadenaConexion);
            var result = await datos.BuscarID(int.Parse(idclientedocumento));
            return Ok(result);

        }
    }
}
