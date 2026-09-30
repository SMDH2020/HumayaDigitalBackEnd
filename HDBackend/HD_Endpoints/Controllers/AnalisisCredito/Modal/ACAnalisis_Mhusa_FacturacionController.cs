using HD.Clientes.Consultas.AnalisisCredito.Modal;
using HD.Clientes.Modelos.SC_Analisis.JDF;
using HD.Security;
using Microsoft.AspNetCore.Mvc;

namespace HD.Endpoints.Controllers.AnalisisCredito.Modal
{
    public class ACAnalisis_Mhusa_FacturacionController : MyBase
    {
        private readonly IConfiguration Configuracion;
        private readonly ISesion Sesion;
        public ACAnalisis_Mhusa_FacturacionController(IConfiguration configuration, ISesion sesion)
        {
            Configuracion = configuration;
            Sesion = sesion;
        }

        [HttpPost]
        public async Task<ActionResult> Guardar(mdlJDFAnalisis_Datos_Facturacion_Guardar mdl)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisisMhusa_Factura datos = new ADAnalisisMhusa_Factura(CadenaConexion);
            mdl.usuario = Sesion.usuario();
            var result = await datos.Guardar(mdl);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]")]
        public async Task<ActionResult> Get(string folio)
        {
            string CadenaConexion = Configuracion["ConnectionStrings:Servicio"];
            ADAnalisisMhusa_Factura datos = new ADAnalisisMhusa_Factura(CadenaConexion);
            var result = await datos.Obtener(folio, Sesion.usuario());
            return Ok(result);

        }
    }
}
