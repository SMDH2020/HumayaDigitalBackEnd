using HD.Clientes.Consultas.AnalisisCredito;
using HD.Generales.Autenticate;
using HD.Generales.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito.AnalisisCredito
{
    public class AnalisisSolicitudCreditoComentariosController : MyBase
    {
        private readonly IConfiguration Configuracion;
        public AnalisisSolicitudCreditoComentariosController(IConfiguration configuration)
        {

            Configuracion = configuration;
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Listado(string folio)
        {
            if (folio == null || folio.Length !=13)
            {
                return BadRequest(new { mensaje = "Los datos proporcionados no son validos" });
            }
                string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
                ADAnalisisComentariosList datos = new ADAnalisisComentariosList(CadenaConexion);
                var result = await datos.Listado(folio);
                return Ok(result);
        }
    }
}
