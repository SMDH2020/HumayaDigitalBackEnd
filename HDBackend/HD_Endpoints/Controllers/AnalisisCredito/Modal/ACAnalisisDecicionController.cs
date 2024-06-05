using HD.Clientes.Consultas.AnalisisCredito.Modal;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Clientes.Modelos.SC_Analisis.Modal;
using HD.Generales.Consultas;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.Modal
{
    public class ACAnalisisDecicionController:MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACAnalisisDecicionController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }
        [HttpPost]
        public async Task<ActionResult> Get(mdlSCAnalisis_Dedidion_View mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisisDecicion datos = new ADAnalisisDecicion(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Get(mdl);
            return Ok(result);

        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Condiciones(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            AD_Analisis_Condiciones_Credito_Timeline datos = new AD_Analisis_Condiciones_Credito_Timeline(CadenaConexion);
            var result = await datos.Condiciones(folio);
            return Ok(result);

        }
    }
}
