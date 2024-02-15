using HD.Clientes.Consultas.SolicitudImpresion;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.Credito
{
    public class SolicitudImpresionController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public SolicitudImpresionController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> PDF(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADSolicitud_Impresion_View datos = new ADSolicitud_Impresion_View(CadenaConexion);
            var result = await datos.Get(folio);
            return Ok(result);

        }
    }
}
