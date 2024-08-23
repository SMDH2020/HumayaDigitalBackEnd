using HD.Clientes.Consultas.AnalisisCredito;
using HD.Clientes.Consultas.AnalisisCredito.Modal;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.Modal
{
    public class ACDocumentacionController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACDocumentacionController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Get(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Documentacion datos = new ADAnalisis_Documentacion(CadenaConexion);
            var result = await datos.Get(folio,Sesion.usuario());
            return Ok(result);

        }
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetOtorgamientoMhusa(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Documentacion datos = new ADAnalisis_Documentacion(CadenaConexion);
            var result = await datos.GetOtorgamientoMhusa(folio, Sesion.usuario());
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> OtorgamientoAgregarDocumento(string folio,int iddocumento)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Documentacion datos = new ADAnalisis_Documentacion(CadenaConexion);
            var result = await datos.SetOtorgamientoMhusaDocumento(folio,iddocumento, Sesion.usuario());
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetAsesor(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Documentacion datos = new ADAnalisis_Documentacion(CadenaConexion);
            var result = await datos.GetAsesor(folio, Sesion.usuario());
            return Ok(result);

        }

        
        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetAsesorCondicionado(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Documentacion datos = new ADAnalisis_Documentacion(CadenaConexion);
            var result = await datos.GetAsesorCondicionado(folio, Sesion.usuario());
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> GetGerenteCondicionado(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisis_Documentacion datos = new ADAnalisis_Documentacion(CadenaConexion);
            var result = await datos.GetGerenteCondicionado(folio, Sesion.usuario());
            return Ok(result);

        }
    }
}
